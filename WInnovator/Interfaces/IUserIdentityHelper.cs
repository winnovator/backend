using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WInnovator.Interfaces
{
    public interface IUserIdentityHelper
    {
        Task<bool> CreateConfirmedUserIfNonExistent(string username, string password);
        Task<bool> AddRoleToUser(string username, string rolename);
        Task<bool> CreateRoleIfNonExistent(string rolename);
        Task<IdentityUser> SearchUser(string username);
        Task<bool> CredentialsAreValid(string username, string password);
        Task<IList<string>> GetAllRolesForUser(IdentityUser user);
        Task<bool> UserHasRole(string username, string rolename);
        Task<bool> RemoveAppUser(string username);
        Task<bool> RemoveAllRolesFromUser(string username);
        string ConstructAppUsername();

        Task<string> GenerateJwtToken(string username);
    }
}
