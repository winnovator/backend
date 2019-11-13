using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WInnovator.Interfaces
{
    public interface IUserIdentityHelper
    {
        Task CreateConfirmedUserIfNonExistent(string username, string password);
        Task AddRoleToUser(string username, string rolename);
        Task CreateRoleIfNonExistent(string rolename);
        Task<IdentityUser> SearchUser(string username);
        Task<bool> CredentialsAreValid(string username, string password);
        Task<IList<string>> GetAllRolesForUser(IdentityUser user);
        Task<bool> UserHasRole(string username, string rolename);
        Task RemoveAppUser(string username);
        Task RemoveAllRolesFromUser(string username);
        string ConstructAppUsername();

        Task<string> GenerateJwtToken(string username);
    }
}
