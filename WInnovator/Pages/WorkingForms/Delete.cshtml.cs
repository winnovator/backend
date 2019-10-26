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

namespace WInnovator.Pages.WorkingForms
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
