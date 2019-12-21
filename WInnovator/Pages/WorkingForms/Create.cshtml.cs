using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator.Pages.WorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class CreateModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;

        public CreateModel(WInnovator.DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["PhaseId"] = new SelectList(_context.Phase, "Id", "Name");

            return Page();
        }

        [BindProperty]
        public WorkingForm WorkingForm { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.WorkingForm.Add(WorkingForm);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
