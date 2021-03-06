﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class IndexModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;

        public IndexModel(WInnovator.DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Phase> Phase { get;set; }

        public async Task OnGetAsync()
        {
            Phase = await _context.Phase.ToListAsync();
        }
    }
}
