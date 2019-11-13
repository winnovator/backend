using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PasswordGenerator;
using System;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Helper;
using WInnovator.Interfaces;

namespace WInnovator.Helper
{
    public class UserIdentityHelper : IUserIdentityHelper
    {
        private IServiceProvider _serviceProvider;
        private ILogger<UserIdentityHelper> _logger;
        private UserManager<IdentityUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public UserIdentityHelper(IServiceProvider serviceProvider, ILogger<UserIdentityHelper> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task CreateConfirmedUserIfNonExistent(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                // If an empty password has been set, generate a secure one
                password = GenerateSecurePassword();
            }

            // Does the user exist?
            if(!(await SearchUser(username)).Exists())
            {
                // No, create it!
                await CreateConfirmedUser(username, password);
            }
        }

        public async Task AddRoleToUser(string username, string roleName)
        {
            IdentityUser user;

            // Does the user exist?
            user = await SearchUser(username);
            if (user.Exists())
            {
                if (!await RoleExists(roleName))
                {
                    _logger.LogError($"Non-existing role { roleName } defined for user { username } ");
                }
                else
                {
                    await AddRoleToUserIfNonExistent(user, roleName);
                }
            }
            else
            {
                // No, create error in log!
                _logger.LogError($"Error, user { username } doesn't exist!");
            }
        }

        public async Task CreateRoleIfNonExistent(string roleName)
        {
            if (!await RoleExists(roleName))
            {
                await CreateRole(roleName);
            }
        }

        public async Task<IdentityUser> SearchUser(string username)
        {
            return await _userManager.FindByEmailAsync(username);
        }

        public async Task RemoveAppUser(string username)
        {
            var user = await SearchUser(username);

            if(user.Exists())
            {
                var logins = await _userManager.GetLoginsAsync(user);
                foreach(var login in logins)
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }
                await _userManager.DeleteAsync(user);
            }
        }

        public string ConstructAppUsername()
        {
            return ConstructAppUsernamePrefix() + DefaultUsersAndRoles.defaultMailPartOfAppUserAccounts;
        }

        public async Task RemoveAllRolesFromUser(string username)
        {
            var user = await SearchUser(username);

            if (user.Exists())
            {
                var roles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            }
        }

        public async Task<bool> UserHasRole(string username, string rolename)
        {
            var user = await SearchUser(username);
            return await UserHasRole(user, rolename);
        }

        private async Task<bool> UserHasRole(IdentityUser user, string rolename)
        {
            return await _userManager.IsInRoleAsync(user, rolename);
        }

        private async Task CreateRole(string roleName)
        {
            IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!identityResult.Succeeded)
            {
                _logger.LogError($"Error creating role { roleName }: { identityResult.Errors.ToString() }");
            }
        }

        private async Task AddRoleToUserIfNonExistent(IdentityUser user, string roleName)
        {
            if (!(await UserHasRole(user, roleName)))
            {
                await GiveUserTheNewRole(user, roleName);
            }
        }

        private async Task GiveUserTheNewRole(IdentityUser user, string roleName)
        {
            IdentityResult identityResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!identityResult.Succeeded)
            {
                _logger.LogError($"Error adding role { roleName } to user { user.Email }: { identityResult.Errors.ToString() }");
            }
        }

        private string GenerateSecurePassword()
        {
            var pwd = new Password().IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric().LengthRequired(64);
            return pwd.Next();

        }

        private async Task CreateConfirmedUser(string username, string password)
        {
            IdentityResult identityResult;
            IdentityUser user;

            user = new IdentityUser() { UserName = username, Email = username, EmailConfirmed = true };
            identityResult = await _userManager.CreateAsync(user, password);
            _logger.LogError($"Error creating user { user.UserName }: { identityResult.Errors.ToString() }");
        }

        private async Task<bool> RoleExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        private string ConstructAppUsernamePrefix()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString("N");
        }
    }
}

public static class UserIdentityExtension
{
    public static bool Exists(this IdentityUser user)
    {
        return user != null;
    }

    public static bool IsAppUserAccount(this IdentityUser user)
    {
        return user.Email.EndsWith(DefaultUsersAndRoles.defaultMailPartOfAppUserAccounts);
    }
}
