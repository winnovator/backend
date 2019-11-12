using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PasswordGenerator;
using System;
using System.Threading.Tasks;
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
                    await AddRoleToUserIfNonExistent(username, roleName, user);
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

        private async Task CreateRole(string roleName)
        {
            IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!identityResult.Succeeded)
            {
                _logger.LogError($"Error creating role { roleName }: { identityResult.Errors.ToString() }");
            }
        }

        private async Task AddRoleToUserIfNonExistent(string username, string roleName, IdentityUser user)
        {
            if (!(await _userManager.IsInRoleAsync(user, roleName)))
            {
                await GiveUserTheNewRole(username, roleName, user);
            }
        }

        private async Task GiveUserTheNewRole(string username, string roleName, IdentityUser user)
        {
            IdentityResult identityResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!identityResult.Succeeded)
            {
                _logger.LogError($"Error adding role { roleName } to user { username }: { identityResult.Errors.ToString() }");
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
    }
}

public static class UserIdentityExtension
{
    public static bool Exists(this IdentityUser user)
    {
        return user != null;
    }
}
