using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WInnovator.Data;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShops
{
    [ExcludeFromCodeCoverage]
    public class CreateModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public CreateModel(WInnovator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DesignShop DesignShop { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.DesignShop.Add(DesignShop);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
