using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShops
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class DeleteModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public DeleteModel(WInnovator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public DesignShop DesignShop { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DesignShop = await _context.DesignShop.FirstOrDefaultAsync(m => m.Id == id);

            if (DesignShop == null)
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

            DesignShop = await _context.DesignShop.FindAsync(id);

            if (DesignShop != null)
            {
                _context.DesignShop.Remove(DesignShop);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
