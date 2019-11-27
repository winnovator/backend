using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.Helper;
using WInnovator.Interfaces;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShops
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class CreateModel : PageModelWithAppUserMethods
    {
        public CreateModel(WInnovator.Data.ApplicationDbContext context, IUserIdentityHelper userIdentityHelper) : base(context, userIdentityHelper)
        {
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public DesignShop DesignShop { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            string createdUser="";

            if (ModelState.IsValid)
            {
                createdUser = await CreateUserForDesignShop();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            DesignShop.AppUseraccount = createdUser;

            _context.DesignShop.Add(DesignShop);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
