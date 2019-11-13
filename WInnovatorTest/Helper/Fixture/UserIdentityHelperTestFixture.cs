using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NLog;
using WInnovator.Helper;
using WInnovatorTest.Data;

namespace WInnovatorTest.Helper.Fixture
{
    public class UserIdentityHelperTestFixture : DbContextTest
    {
        public ILogger<UserIdentityHelper> _logger;
        public Mock<IUserStore<IdentityUser>> _userStore;
        public UserManager<IdentityUser> _userManager;
        public Mock<IRoleStore<IdentityRole>> _roleStore;
        public RoleManager<IdentityRole> _roleManager;
        public Mock<IConfiguration> _configuration;
        public UserIdentityHelper _controller;

        public UserIdentityHelperTestFixture()
        {
            // For mocking Usermanager, see https://stackoverflow.com/questions/49165810/how-to-mock-usermanager-in-net-core-testing
            _logger = Mock.Of<ILogger<UserIdentityHelper>>();
            _userStore = new Mock<IUserStore<IdentityUser>>();
            _userManager = new UserManager<IdentityUser>(_userStore.Object, null, null, null, null, null, null, null, null);
            _roleStore = new Mock<IRoleStore<IdentityRole>>();
            _roleManager = new RoleManager<IdentityRole>(_roleStore.Object, null, null, null, null);
            
            _configuration = new Mock<IConfiguration>();
            
            _controller = new UserIdentityHelper(_applicationTestDbContext, _logger, _userManager, _roleManager, _configuration.Object);
        }
    }
}