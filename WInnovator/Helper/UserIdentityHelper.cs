using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using PasswordGenerator;
using WInnovator.Data;
using WInnovator.Helper;
using WInnovator.Interfaces;

namespace WInnovator.Helper
{
    public class UserIdentityHelper : IUserIdentityHelper
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserIdentityHelper> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public UserIdentityHelper(ApplicationDbContext context, ILogger<UserIdentityHelper> logger, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        #region "Public methods defined in the interface"
        public async Task<bool> CreateConfirmedUserIfNonExistent(string username, string password)
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
                return await CreateConfirmedUser(username, password);
            }

            return true;
        }

        public async Task<bool> AddRoleToUser(string username, string roleName)
        {
            IdentityUser user;

            // Does the user exist?
            user = await SearchUser(username);
            if (user.Exists())
            {
                if (!await RoleExists(roleName))
                {
                    _logger.LogError("Non-existing role { roleName } defined for user { username } ", roleName, username);
                    return false;
                }
                else
                {
                    return await AddRoleToUserIfNonExistent(user, roleName);
                }
            }
            else
            {
                // No, create error in log!
                _logger.LogError("Error, user { username } doesn't exist!", username);
                return false;
            }
        }

        public async Task<bool> CreateRoleIfNonExistent(string roleName)
        {
            if (!await RoleExists(roleName))
            {
                return await CreateRole(roleName);
            }

            return true;
        }

        public async Task<IdentityUser> SearchUser(string username)
        {
            return await _userManager.FindByEmailAsync(username);
        }

        public async Task<bool> CredentialsAreValid(string username, string password)
        {
            var user = await SearchUser(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IList<string>> GetAllRolesForUser(IdentityUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> RemoveAppUser(string username)
        {
            var user = await SearchUser(username);

            if(user.Exists())
            {
                var logins = await _userManager.GetLoginsAsync(user);
                foreach(var login in logins)
                {
                    await _userManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
                }
                IdentityResult identityResult = await _userManager.DeleteAsync(user);
                if (!identityResult.Succeeded)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public string ConstructAppUsername()
        {
            return ConstructAppUsernamePrefix() + DefaultUsersAndRoles.defaultMailPartOfAppUserAccounts;
        }

        public async Task<bool> RemoveAllRolesFromUser(string username)
        {
            var user = await SearchUser(username);

            if (user.Exists())
            {
                var roles = await GetAllRolesForUser(user);
                IdentityResult identityResult =  await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
                if (!identityResult.Succeeded)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        [ExcludeFromCodeCoverage]
        public async Task<bool> UserHasRole(string username, string rolename)
        {
            var user = await SearchUser(username);
            return await UserHasRole(user, rolename);
        }

        [ExcludeFromCodeCoverage]
        public async Task<string> GenerateJwtToken(string username)
        {
            // Source: https://www.youtube.com/watch?v=9QU_y7-VsC8
            var user = await SearchUser(username);
            var roles = await GetAllRolesForUser(user);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                await GenerateNotBeforeClaim(user),
                await GenerateExpiresAfterClaim(user)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                                SecurityAlgorithms.HmacSha256)),
                                new JwtPayload(claims));

            _logger.LogTrace("Token generated for user { username } of type { usertype }", username, (user.IsAppUserAccount() ? "AppUseraccount" : "normal account"));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion
        
        private async Task<Claim> GenerateNotBeforeClaim(IdentityUser user)
        {
            if(user.IsAppUserAccount())
            {
                return new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(await GetDesignShopDate(user)).ToUnixTimeSeconds().ToString());
            }
            return new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString());
        }

        private async Task<Claim> GenerateExpiresAfterClaim(IdentityUser user)
        {
            if (user.IsAppUserAccount())
            {
                return new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset((await GetDesignShopDate(user)).Date.AddDays(1).AddTicks(-1)).ToUnixTimeSeconds().ToString());
            }
            return new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString());
        }

        private async Task<DateTime> GetDesignShopDate(IdentityUser user)
        {
            return (await _context.DesignShop.Where(shop => shop.AppUseraccount == user.Email).FirstAsync()).Date;
        }

        private async Task<bool> UserHasRole(IdentityUser user, string rolename)
        {
            return await _userManager.IsInRoleAsync(user, rolename);
        }

        private async Task<bool> CreateRole(string roleName)
        {
            IdentityResult identityResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (!identityResult.Succeeded)
            {
                _logger.LogError("Error creating role { roleName }", roleName);
                return false;
            }

            _logger.LogTrace("Role { roleName } created.", roleName);
            return true;
        }

        [ExcludeFromCodeCoverage]
        private async Task<bool> AddRoleToUserIfNonExistent(IdentityUser user, string roleName)
        {
            if (!(await UserHasRole(user, roleName)))
            {
                return await GiveUserTheNewRole(user, roleName);
            }

            return true;
        }

        private async Task<bool> GiveUserTheNewRole(IdentityUser user, string roleName)
        {
            IdentityResult identityResult = await _userManager.AddToRoleAsync(user, roleName);
            if (!identityResult.Succeeded)
            {
                _logger.LogError("Error adding role { roleName } to user { user }", roleName, user.Email);
                return false;
            }

            _logger.LogTrace("Role { roleName } added to user { user }.", roleName, user.Email);
            return true;
        }

        private string GenerateSecurePassword()
        {
            var pwd = new Password().IncludeLowercase().IncludeUppercase().IncludeSpecial().IncludeNumeric().LengthRequired(64);
            return pwd.Next();

        }

        private async Task<bool> CreateConfirmedUser(string username, string password)
        {
            //IdentityResult identityResult;
            // IdentityUser user;

            var user = new IdentityUser() { UserName = username, Email = username, EmailConfirmed = true };
            var identityResult = await _userManager.CreateAsync(user, password);
            if(!identityResult.Succeeded)
            {
                _logger.LogError("Error creating user { user }: { identityResultErrors }", user.UserName, identityResult.Errors.ToString());
                return false;
            }

            _logger.LogTrace("User { user } created. ", user.UserName);
            return true;
        }

        private async Task<bool> RoleExists(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        [ExcludeFromCodeCoverage]
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
