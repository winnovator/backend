using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using WInnovator.Interfaces;

namespace WInnovator.Pages.DesignShops
{
    [ExcludeFromCodeCoverage]
    public class PageModelWithAppUserMethods : PageModel
    {
        protected readonly WInnovator.DAL.ApplicationDbContext _context;
        protected readonly IUserIdentityHelper _userIdentityHelper;

        public PageModelWithAppUserMethods(WInnovator.DAL.ApplicationDbContext context, IUserIdentityHelper userIdentityHelper)
        {
            _context = context;
            _userIdentityHelper = userIdentityHelper;
        }

        protected async Task<string> CreateUserForDesignShop()
        {
            string userName = _userIdentityHelper.ConstructAppUsername();
            await _userIdentityHelper.CreateConfirmedUserIfNonExistent(userName, "");
            if (!(await _userIdentityHelper.SearchUser(userName)).Exists())
            {
                ModelState.AddModelError("User", "Useraccount cannot be created");
                return "";
            }
            await _userIdentityHelper.AddRoleToUser(userName, "FrontendApp");
            if (!await _userIdentityHelper.UserHasRole(userName, "FrontendApp"))
            {
                ModelState.AddModelError("Role", "Role cannot be added to the account");
                return "";
            }
            return userName;
        }

        protected async Task RemoveAppUseraccountIfExisting(string username)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                await _userIdentityHelper.RemoveAllRolesFromUser(username);
                await _userIdentityHelper.RemoveAppUser(username);
            }
        }
    }
}
