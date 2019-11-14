using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using WInnovator.Helper;
using WInnovator.Models;
using WInnovatorTest.Data;

namespace WInnovatorTest.Helper.Fixture
{
    public class UserIdentityHelperTestFixture : DbContextTest
    {
        public Mock<ILogger<UserIdentityHelper>> _logger;
        public Mock<IUserStore<IdentityUser>> _userStore;
        public Mock<UserManager<IdentityUser>> _userManager;
        public Mock<IRoleStore<IdentityRole>> _roleStore;
        public Mock<RoleManager<IdentityRole>> _roleManager;
        public Mock<IConfiguration> _configuration;
        public UserIdentityHelper _controller;

        public DesignShop _shopWithAppUser;
        public IdentityUser _appUser;
        public string _appUserRole;
        public IdentityUser _normalUser;
        public string _validPassword;
        public string _newUserName;
        public IList<string> _appUserRoles;

        public UserIdentityHelperTestFixture()
        {
            // For mocking Usermanager, see https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
            _logger = new Mock<ILogger<UserIdentityHelper>>();
            _userStore = new Mock<IUserStore<IdentityUser>>();
            _userManager = new Mock<UserManager<IdentityUser>>(_userStore.Object, null, null, null, null, null, null, null, null);
            _roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManager = new Mock<RoleManager<IdentityRole>>(_roleStore.Object, null, null, null, null);
            _configuration = new Mock<IConfiguration>();
            _controller = new UserIdentityHelper(_applicationTestDbContext, _logger.Object, _userManager.Object, _roleManager.Object, _configuration.Object);
            
            MockAppUserWithDesignShop();
            MockNormalUser();
            MockRoleManager();
        }

        private void MockAppUserWithDesignShop()
        {
            // Setup access to private method
            object o = _controller;
            var t = typeof(UserIdentityHelper);
            
            // Construct an app username and designshop
            string username = ((string) t.GetMethod("ConstructAppUsernamePrefix", BindingFlags.Instance | BindingFlags.NonPublic).Invoke(o, new object[] { })) + DefaultUsersAndRoles.defaultMailPartOfAppUserAccounts;
            DateTime date = DateTime.Now.AddDays(5);
            _appUserRoles = new List<string>(new string[] {_appUserRole, "AndAnother"});
            IList<UserLoginInfo> userLoginInfo = new List<UserLoginInfo>(new UserLoginInfo[]
                {new UserLoginInfo("provider", "key", "displayname")});
            
            _shopWithAppUser = new DesignShop() { Date = date, AppUseraccount = username };
            _applicationTestDbContext.DesignShop.Add(_shopWithAppUser);
            _appUser = new IdentityUser() { Email = username };
            _appUserRole = "appUserRole";
            _applicationTestDbContext.SaveChanges();

            _userManager.Setup(um => um.IsInRoleAsync(_appUser, It.IsAny<string>())).ReturnsAsync(false);
            _userManager.Setup(um => um.IsInRoleAsync(_appUser, _appUserRole)).ReturnsAsync(true);
            
            _userManager.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((IdentityUser)null);
            _userManager.Setup(um => um.FindByEmailAsync(_appUser.Email)).ReturnsAsync(_appUser);

            _userManager.Setup(um => um.GetRolesAsync(It.Is<IdentityUser>(user => _appUser.Email.Equals(user.Email))))
                .ReturnsAsync(_appUserRoles);

            _userManager.Setup(um => um.GetLoginsAsync(It.Is<IdentityUser>(user => _appUser.Email.Equals(user.Email))))
                .ReturnsAsync(userLoginInfo);
            
            _userManager.Setup(um => um.RemoveLoginAsync(It.Is<IdentityUser>(user => _appUser.Email.Equals(user.Email)), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(um => um.DeleteAsync(It.Is<IdentityUser>(user => _appUser.Email.Equals(user.Email))))
                .ReturnsAsync(IdentityResult.Success);
            
            _userManager.Setup(um => um.RemoveFromRolesAsync(It.Is<IdentityUser>(user => _appUser.Email.Equals(user.Email)), It.IsAny<string[]>()))
                .ReturnsAsync(IdentityResult.Success);

        }
        
        private void MockNormalUser()
        {
            IList<UserLoginInfo> userLoginInfo = new List<UserLoginInfo>(new UserLoginInfo[]
                {new UserLoginInfo("provider", "key", "displayname")});

            _normalUser = new IdentityUser() { Email = @"test@test.test"};
            _validPassword = "validPassword01";
            _userManager
                .Setup(um => um.CreateAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email)),
                    It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager
                .Setup(um => um.CreateAsync(It.Is<IdentityUser>(user => !_normalUser.Email.Equals(user.Email)),
                    It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());
            _userManager
                .Setup(um =>
                    um.AddToRoleAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email)), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed());
            _userManager
                .Setup(um =>
                    um.AddToRoleAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email)), _appUserRole))
                .ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(um => um.FindByEmailAsync(_normalUser.Email)).ReturnsAsync(_normalUser);
            _userManager.Setup(um => um.CheckPasswordAsync(_normalUser, It.IsAny<string>())).ReturnsAsync(false);
            _userManager.Setup(um => um.CheckPasswordAsync(_normalUser, _validPassword)).ReturnsAsync(true);

            _userManager.Setup(um => um.GetRolesAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email))))
                .ReturnsAsync(_appUserRoles);

            _userManager.Setup(um => um.GetLoginsAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email))))
                .ReturnsAsync(userLoginInfo);
            
            _userManager.Setup(um => um.RemoveLoginAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email)), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            _userManager.Setup(um => um.DeleteAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email))))
                .ReturnsAsync(IdentityResult.Failed());
            
            _userManager.Setup(um => um.RemoveFromRolesAsync(It.Is<IdentityUser>(user => _normalUser.Email.Equals(user.Email)), It.IsAny<string[]>()))
                .ReturnsAsync(IdentityResult.Failed());


        }

        private void MockRoleManager()
        {
            _roleManager.Setup(rm => rm.CreateAsync(It.Is<IdentityRole>(role=> _appUserRole.Equals(role.Name))))
                .ReturnsAsync(IdentityResult.Failed());
            _roleManager.Setup(rm => rm.CreateAsync(It.Is<IdentityRole>(role=> !_appUserRole.Equals(role.Name))))
                .ReturnsAsync(IdentityResult.Success);

            _roleManager.Setup(rm => rm.RoleExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            _roleManager.Setup(rm => rm.RoleExistsAsync(_appUserRole)).ReturnsAsync(true);
        }
    }
}