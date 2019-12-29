using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator.Pages.WorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class CreateModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(WInnovator.DAL.ApplicationDbContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult OnGet()
        {
            ViewData["PhaseId"] = new SelectList(_context.Phase, "Id", "Name");
            WorkingForm WorkingForm = new WorkingForm();
            if (TempData["belongsToDesignShop"] != null)
            {
                try
                {
                    Guid designshopId = Guid.Parse(TempData["belongsToDesignShop"].ToString());
                    DesignShop designShop = _context.DesignShop.Where(ds => ds.Id == designshopId).FirstOrDefault();
                    if (designShop != null)
                    {
                        ViewData["DesignShopDescription"] = designShop.Description;
                        TempData["belongsToDesignShop"] = designshopId.ToString();
                    }
                }
                catch
                {
                    _logger.LogError("Something went wrong while extracting the GUID with value {value }", TempData["belongsToDesignShop"]);
                    ViewData["GuidParseError"] = "Unable to get the ID of the DesignShop";
                }
            }

            return Page();
        }

        [BindProperty]
        public WorkingForm WorkingForm { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Guid designshopId = Guid.Empty;

            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (TempData["belongsToDesignShop"] != null)
            {
                try
                {
                    designshopId = Guid.Parse(TempData["belongsToDesignShop"].ToString());
                    WorkingForm.belongsToDesignShopId = designshopId;
                }
                catch
                {
                    _logger.LogError("Error while connecting workform { workformid } to its parent.", WorkingForm.Id);
                }
            }

            _context.WorkingForm.Add(WorkingForm);
            await _context.SaveChangesAsync();

            if(designshopId != Guid.Empty)
            {
                // There is a GUID of the designshop, so we'll create a DesignShopWorkingForm
                DesignShopWorkingForm designShopWorkingForm = new DesignShopWorkingForm();
                designShopWorkingForm.DesignShopId = designshopId;
                designShopWorkingForm.WorkingFormId = WorkingForm.Id;
                designShopWorkingForm.DisplayName = WorkingForm.DisplayName;
                designShopWorkingForm.Implementer = "UNKNOWN";
                designShopWorkingForm.TimeAllocated = WorkingForm.DefaultTimeNeeded;
                designShopWorkingForm.PhaseId = WorkingForm.PhaseId;
                designShopWorkingForm.UploadEnabled = WorkingForm.UploadEnabled;
                designShopWorkingForm.Resume = WorkingForm.Resume;
                designShopWorkingForm.Description = WorkingForm.Description;
                designShopWorkingForm.IsCurrentWorkingForm = false;
                designShopWorkingForm.Order = getCorrectFirstPosition(designshopId);

                _context.DesignShopWorkingForm.Add(designShopWorkingForm);
                await _context.SaveChangesAsync();

                TempData["selectedDesignShop"] = designshopId;
                return RedirectToPage("../DesignShopsWorkingForms/Index");
            }

            return RedirectToPage("./Index");
        }

        private int getCorrectFirstPosition(Guid designShopId)
        {
            // The first position is equal to the highest order + 1, or just 1 if no other workingform is existing
            var designshop = _context.DesignShopWorkingForm.Where(dswf => dswf.DesignShopId == designShopId).OrderByDescending(dswf => dswf.Order).FirstOrDefault();
            if (designshop == null)
            {
                return 1;
            }
            return designshop.Order + 1;
        }
    }
}
