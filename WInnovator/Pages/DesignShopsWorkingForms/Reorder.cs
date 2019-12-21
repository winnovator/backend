using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.DAL;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShopsWorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class ReorderModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReorderModel> _logger;

        public List<DesignShopWorkingForm> DesignShopWorkingForms;
        public List<DesignShopWorkingForm> ArrangeddDesignShopWorkingForms;

        public ReorderModel(ApplicationDbContext context, ILogger<ReorderModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> OnGetAsync(Guid? designshopId)
        {
            if (designshopId == null)
            {
                return NotFound();
            }

            DesignShopWorkingForms = await _context.DesignShopWorkingForm
                .Where(dswf=>dswf.DesignShopId == designshopId)
                .Include(d => d.DesignShop)
                .Include(d => d.WorkingForm)
                .ToListAsync();

            ArrangeddDesignShopWorkingForms = DesignShopWorkingForms.OrderBy(x => x.Order).ToList();

            TempData["selectedDesignShop"] = designshopId;

            return Page();
        }

    }
}
