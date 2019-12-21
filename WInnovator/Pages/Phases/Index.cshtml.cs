using System;
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
