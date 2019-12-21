using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WInnovator.Data;
using WInnovator.Models;

namespace WInnovator
{
    public class IndexModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public IndexModel(WInnovator.Data.ApplicationDbContext context)
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
