using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using WInnovator.API;
using WInnovator.Models;
using WInnovatorTest.Data;
using Xunit;

namespace WInnovatorTest.API
{
    public class DesignShopControllerTest : DbContextTest
    {
        [Fact]
        public async Task DesignShopCreate()
        {
            // Arrange
            ILogger<DesignShopController> logger = Mock.Of<ILogger<DesignShopController>>();
            var controller = new DesignShopController(_applicationTestDbContext, logger);

            // Act
            var result = await controller.CreateDesignShop();

            // Assert
            // Peel off seperate layers
            var firstResult = Assert.IsType<ActionResult<DesignShop>>(result);
            var secondResult = Assert.IsType<CreatedAtActionResult>(firstResult.Result);
            // Finally we get the desired value, cast it to use it
            var designShop = (DesignShop)secondResult.Value;
            // Assert that we got an ID
            Assert.False(string.IsNullOrEmpty(designShop.Id.ToString()));

        }
    }
}
