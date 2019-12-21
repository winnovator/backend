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

namespace WInnovator.Pages.DesignShopsWorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class EditModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public EditModel(WInnovator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DesignShopWorkingForm DesignShopWorkingForm { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DesignShopWorkingForm = await _context.DesignShopWorkingForm
                .Include(d => d.DesignShop)
                .Include(d => d.WorkingForm).FirstOrDefaultAsync(m => m.Id == id);

            if (DesignShopWorkingForm == null)
            {
                return NotFound();
            }
           ViewData["DesignShopId"] = new SelectList(_context.DesignShop.Where(ds => ds.Id == DesignShopWorkingForm.DesignShopId), "Id", "Description");
           ViewData["WorkingFormId"] = new SelectList(_context.WorkingForm, "Id", "Name");
           TempData["selectedDesignShop"] = DesignShopWorkingForm.DesignShopId;

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

            _context.Attach(DesignShopWorkingForm).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesignShopWorkingFormExists(DesignShopWorkingForm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["selectedDesignShop"] = DesignShopWorkingForm.DesignShopId;
            return RedirectToPage("./Index");
        }

        private bool DesignShopWorkingFormExists(Guid id)
        {
            return _context.DesignShopWorkingForm.Any(e => e.Id == id);
        }
    }
}
