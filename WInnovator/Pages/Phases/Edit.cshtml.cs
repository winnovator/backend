using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class EditModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;

        public EditModel(WInnovator.DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Phase Phase { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Phase = await _context.Phase.FirstOrDefaultAsync(m => m.Id == id);

            if (Phase == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Phase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhaseExists(Phase.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PhaseExists(Guid id)
        {
            return _context.Phase.Any(e => e.Id == id);
        }
    }
}
