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
        Task AddRoleToUser(string username, string roleName);
        Task CreateRoleIfNonExistent(string roleName);
        Task<IdentityUser> SearchUser(string username);
    }
}
