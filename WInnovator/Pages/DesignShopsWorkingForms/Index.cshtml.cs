using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WInnovator.Data;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShopsWorkingForms
{
    [ExcludeFromCodeCoverage]
    public class IndexModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public IndexModel(WInnovator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<DesignShopWorkingForm> DesignShopWorkingForm { get;set; }
        public IList<DesignShop> listOfDesignShop { get; set; }
        public SelectList DesignShops { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> CurrentDesignShop { get; set; }

        public async Task OnGetAsync()
        {
            LoadDesignShops();
            DesignShop first = listOfDesignShop.FirstOrDefault();
            if(first != null)
            { 
                await GetWorkingForms(first.Id);
            }

            DesignShops = new SelectList(_context.DesignShop, nameof(DesignShop.Id), nameof(DesignShop.Description));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            LoadDesignShops();

            if(!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Guid dsGuid = Guid.Parse(ModelState.Values.ToList().First().AttemptedValue);
                await GetWorkingForms(dsGuid);

                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task GetWorkingForms(Guid dsGuid) {
            DesignShopWorkingForm = await _context.DesignShopWorkingForm
                .Include(d => d.DesignShop)
                .Include(d => d.WorkingForm)
                .Where(d => d.DesignShop.Id == dsGuid)
                .ToListAsync();

        }

        private void LoadDesignShops()
        {
            listOfDesignShop = _context.DesignShop.Where(ds => ds.Date >= DateTime.UtcNow).OrderBy(ds => ds.Date).ToList();
            DesignShops = new SelectList(listOfDesignShop.Where(ds => ds.Date >= DateTime.UtcNow).OrderBy(ds => ds.Date), nameof(DesignShop.Id), nameof(DesignShop.Description));
        }
    }
}
