using System;
using Microsoft.Extensions.Logging;
using Moq;
using WInnovator.API;
using WInnovator.Interfaces;
using WInnovator.Models;
using WInnovatorTest.Data;

namespace WInnovatorTest.API.Fixtures
{
    public class DesignShopControllerTestFixture : DbContextTest
    {
        public ILogger<DesignShopController> _logger;
        public Mock<IUserIdentityHelper> _userIdentityHelper;
        public DesignShopController _controller;
        public DesignShop _designShop;

        public DesignShopControllerTestFixture()
        {
            // Arrange
            _logger = Mock.Of<ILogger<DesignShopController>>();
            _userIdentityHelper = new Mock<IUserIdentityHelper>();
            _userIdentityHelper.Setup(uih => uih.GenerateJwtToken(It.IsAny<string>())).ReturnsAsync((string s) => s);
            _controller = new DesignShopController(_applicationTestDbContext, _logger, _userIdentityHelper.Object);
            MockDesignShops();
        }

        private void MockDesignShops()
        {
            // Setup several empty designshops
            for (var i = 0; i < 6; i++)
            {
                _applicationTestDbContext.DesignShop.Add(new DesignShop() {Date = DateTime.Now.AddDays(-i)});
            }

            // Add one we'll remember
            _designShop = new DesignShop() {Date = DateTime.Now, Description = "Remember this!", AppUseraccount = "AppUseraccount"};
            _applicationTestDbContext.DesignShop.Add(_designShop);

            // And throw in another empty designshops
            for (var i = 0; i < 6; i++)
            {
                _applicationTestDbContext.DesignShop.Add(new DesignShop() {Date = DateTime.Now.AddDays(-i)});
            }

            _applicationTestDbContext.SaveChanges();
        }
    }
}