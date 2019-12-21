using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WInnovator.DAL;
using WInnovator.Models;

namespace WInnovator
{
    public class CreateModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;

        public CreateModel(WInnovator.DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Phase Phase { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Phase.Add(Phase);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
