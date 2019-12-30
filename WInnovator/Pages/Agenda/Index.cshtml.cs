using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.API;
using WInnovator.Models;

namespace WInnovator.Pages.Agenda
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class IndexModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> _logger;
        private readonly ILogger<WorkingFormController> _workingFormControllerLogger;

        public IndexModel(WInnovator.DAL.ApplicationDbContext context, ILogger<IndexModel> logger, ILogger<WorkingFormController> workingFormControllerLogger)
        {
            _context = context;
            _logger = logger;
            _workingFormControllerLogger = workingFormControllerLogger;
        }

        public IList<DesignShopWorkingForm> DesignShopWorkingForm { get; set; }
        public IList<DesignShop> listOfDesignShop { get; set; }
        public SelectList DesignShops { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> CurrentDesignShop { get; set; }
        public Guid currentDesignShopGuid { get; set; }

        public async Task OnGetAsync()
        {
            await GetData();
        }
        private async Task GetData()
        {
            LoadDesignShops();
            DesignShop selected = null;
            if (TempData["selectedDesignShop"] != null)
            {
                try
                {
                    Guid selectedDesignShop = Guid.Parse(TempData["selectedDesignShop"].ToString());
                    if (selectedDesignShop != null && listOfDesignShop.Where(ds => ds.Id == selectedDesignShop).Count() > 0)
                    {
                        selected = listOfDesignShop.Where(ds => ds.Id == selectedDesignShop).First();
                    }
                }
                catch
                {
                    _logger.LogError("Exception thrown when trying to get the selectedDesignShopId from TempData");
                }
            }
            if (selected == null)
            {
                selected = listOfDesignShop.FirstOrDefault();
            }
            if (selected != null)
            {
                currentDesignShopGuid = selected.Id;
                await GetWorkingForms(selected.Id);
            }

            if (selected != null)
            {
                DesignShops = new SelectList(listOfDesignShop, nameof(DesignShop.Id), nameof(DesignShop.Description), selected.Id);
            }
            else
            {
                DesignShops = new SelectList(listOfDesignShop, nameof(DesignShop.Id), nameof(DesignShop.Description));
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadDesignShops();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Guid dsGuid = Guid.Parse(ModelState.Values.ToList().First().AttemptedValue);
                currentDesignShopGuid = dsGuid;
                TempData["selectedDesignShop"] = currentDesignShopGuid;
                await GetWorkingForms(dsGuid);

                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostNextAsync(Guid? designshopId)
        {

            if (designshopId == null)
            {
                return NotFound();
            }

            WorkingFormController controller = new WorkingFormController(_context, _workingFormControllerLogger);

            await controller.SetNextWorkingFormOfDesignShopINTERNAL(designshopId.GetValueOrDefault());

            TempData["selectedDesignShop"] = designshopId;

            return RedirectToPage("./Index");
        }

        public async Task GetWorkingForms(Guid dsGuid)
        {
            DesignShopWorkingForm = await _context.DesignShopWorkingForm
                .Include(d => d.DesignShop)
                .Include(d => d.WorkingForm)
                .Include(d => d.Phase)
                .Where(d => d.DesignShop.Id == dsGuid)
                .OrderBy(d => d.Order)
                .ToListAsync();

        }

        private void LoadDesignShops()
        {
            listOfDesignShop = _context.DesignShop.Where(ds => ds.Date >= DateTime.UtcNow.Date).OrderBy(ds => ds.Date).ToList();
            DesignShops = new SelectList(listOfDesignShop, nameof(DesignShop.Id), nameof(DesignShop.Description));
        }
    }
}
