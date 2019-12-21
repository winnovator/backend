using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator.Pages.WorkingForms
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
        public WorkingForm WorkingForm { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            WorkingForm = await _context.WorkingForm.FirstOrDefaultAsync(m => m.Id == id);

            if (WorkingForm == null)
            {
                return NotFound();
            }
            ViewData["PhaseId"] = new SelectList(_context.Phase, "Id", "Name");

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

            _context.Attach(WorkingForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkingFormExists(WorkingForm.Id))
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

        private bool WorkingFormExists(Guid id)
        {
            return _context.WorkingForm.Any(e => e.Id == id);
        }
    }
}
