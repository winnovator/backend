﻿using System;
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
    public class DeleteModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public DeleteModel(WInnovator.Data.ApplicationDbContext context)
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

            DesignShopWorkingForm = await _context.DesignShopWorkingForm.FindAsync(id);

            if (DesignShopWorkingForm != null)
            {
                _context.DesignShopWorkingForm.Remove(DesignShopWorkingForm);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}