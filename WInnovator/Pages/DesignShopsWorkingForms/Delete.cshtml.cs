﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
    public class DeleteModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;

        public DeleteModel(WInnovator.DAL.ApplicationDbContext context)
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DesignShopWorkingForm = await _context.DesignShopWorkingForm.Where(dswf => dswf.Id == id).Include(dswf => dswf.WorkingForm).FirstOrDefaultAsync();

            if (DesignShopWorkingForm != null)
            {
                // First, check the parent... If the field belongsTo has been set, remove it as well.
                if (DesignShopWorkingForm.WorkingForm.belongsToDesignShopId != null)
                {
                    _context.WorkingForm.Remove(DesignShopWorkingForm.WorkingForm);
                }
                _context.DesignShopWorkingForm.Remove(DesignShopWorkingForm);
                await _context.SaveChangesAsync();
            }

            TempData["selectedDesignShop"] = DesignShopWorkingForm.DesignShopId;
            return RedirectToPage("./Index");
        }
    }
}
