using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class DetailsModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;

        public DetailsModel(WInnovator.DAL.ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
