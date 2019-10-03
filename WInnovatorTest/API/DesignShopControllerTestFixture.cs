using Microsoft.Extensions.Logging;
using Moq;
using WInnovator.API;
using WInnovator.Models;
using WInnovatorTest.Data;

namespace WInnovatorTest.API
{
    public class DesignShopControllerTestFixture : DbContextTest
    {
        public ILogger<DesignShopController> _logger;
        public DesignShopController _controller;
        public DesignShop _createdDesignShop;

        public DesignShopControllerTestFixture()
        {
            // Arrange
            _logger = Mock.Of<ILogger<DesignShopController>>();
            _controller = new DesignShopController(_applicationTestDbContext, _logger);
        }

    }
}
