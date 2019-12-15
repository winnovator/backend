using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator.Pages.WorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class IndexModel : PageModel
    {
        private readonly WInnovator.Data.ApplicationDbContext _context;

        public IndexModel(WInnovator.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<WorkingForm> WorkingForm { get;set; }

        public async Task OnGetAsync()
        {
            WorkingForm = await _context.WorkingForm.ToListAsync();
        }
    }
}
