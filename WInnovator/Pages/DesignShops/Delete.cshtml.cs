using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Interfaces;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShops
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class DeleteModel : PageModelWithAppUserMethods
    {
        public DeleteModel(WInnovator.DAL.ApplicationDbContext context, IUserIdentityHelper userIdentityHelper) : base(context, userIdentityHelper)
        {
        }

        [BindProperty]
        public DesignShop DesignShop { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DesignShop = await _context.DesignShop.FirstOrDefaultAsync(m => m.Id == id);

            if (DesignShop == null)
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

            // Get the current DesignShop with all it's children, including the WorkingForms
            DesignShop = await _context.DesignShop.Where(ds => ds.Id == id).Include(ds => ds.DesignShopWorkingForms).ThenInclude(dswf => dswf.WorkingForm).ThenInclude(wf => wf.DesignShopWorkingForms).FirstOrDefaultAsync();

            if (DesignShop != null)
            {
                await RemoveAppUseraccountIfExisting(DesignShop.AppUseraccount);

                foreach (DesignShopWorkingForm dswf in DesignShop.DesignShopWorkingForms)
                {
                    // First, remove all images from the imagestore
                    _context.ImageStore.RemoveRange(_context.ImageStore.Where(image => image.DesignShopWorkingFormId == dswf.Id));

                    // Then check if the WorkingForm belongs to this DesignShop and there are no other instances. If so, delete it!
                    if (dswf.WorkingForm.belongsToDesignShopId != null && dswf.WorkingForm.DesignShopWorkingForms.Count == 1)
                    {
                        _context.WorkingForm.Remove(dswf.WorkingForm);
                    }
                    // Delete the DesignShopWorkingForm itself
                    _context.DesignShopWorkingForm.Remove(dswf);
                }

                // Finally, delete de DesignShop
                _context.DesignShop.Remove(DesignShop);
                // And commit everything
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
