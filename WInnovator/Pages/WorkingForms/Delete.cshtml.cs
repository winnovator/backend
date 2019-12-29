using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator.Pages.WorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class DeleteModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;

        public DeleteModel(WInnovator.DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public WorkingForm WorkingForm { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkingForm = await _context.WorkingForm.Include(wf => wf.Phase).FirstOrDefaultAsync(m => m.Id == id);

            if (WorkingForm == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkingForm = await _context.WorkingForm.FindAsync(id);

            if (WorkingForm != null)
            {
                _context.WorkingForm.Remove(WorkingForm);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
