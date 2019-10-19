using Microsoft.Extensions.Logging;
using Moq;
using System;
using WInnovator.API;
using WInnovator.Models;
using WInnovatorTest.Data;

namespace WInnovatorTest.API.Fixtures
{
    public class DesignShopControllerTestFixture : DbContextTest
    {
        public ILogger<DesignShopController> _logger;
        public DesignShopController _controller;
        public DesignShop _designShop;

        public DesignShopControllerTestFixture()
        {
            // Arrange
            _logger = Mock.Of<ILogger<DesignShopController>>();
            _controller = new DesignShopController(_applicationTestDbContext, _logger);
            MockDesignShops();
        }

        private void MockDesignShops()
        {
            // Setup several empty designshops
            for (var i = 0; i < 6; i++)
            {
                _applicationTestDbContext.DesignShop.Add(new DesignShop() {Date = DateTime.Now});
            }

            // Add one we'll remember
            _designShop = new DesignShop() {Date = DateTime.Now, Description = "Remember this!"};
            _applicationTestDbContext.DesignShop.Add(_designShop);

            // And throw in another empty designshops
            for (var i = 0; i < 6; i++)
            {
                _applicationTestDbContext.DesignShop.Add(new DesignShop() {Date = DateTime.Now});
            }

            _applicationTestDbContext.SaveChanges();
        }
    }
}