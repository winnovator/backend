using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WInnovator.Data;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShopsWorkingForms
{
    [ExcludeFromCodeCoverage]
    public class DetailsModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public DetailsModel(WInnovator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

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
            return Page();
        }
    }
}
