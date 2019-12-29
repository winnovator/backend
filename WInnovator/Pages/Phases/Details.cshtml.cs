﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WInnovator.DAL;
using WInnovator.Models;

namespace WInnovator
{
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