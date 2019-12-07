using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Helper;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShopsWorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class CreateModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public CreateModel(WInnovator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid? designshopId)
        {
            // Only the current designshop can be selected, no changes can be made!
            ViewData["DesignShopId"] = new SelectList(_context.DesignShop.Where(ds => ds.Id == designshopId), "Id", "Description");
            ViewData["WorkingFormId"] = new SelectList(_context.WorkingForm, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public DesignShopWorkingForm DesignShopWorkingForm { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DesignShopWorkingForm.Add(DesignShopWorkingForm);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
