using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WInnovator.Models;
using WInnovator.ViewModels;
using Xunit;

// See https://xunit.net/docs/shared-context for use of Fixture! (Has to be documented in SAD)

namespace WInnovatorTest.API
{
    [TestCaseOrderer("WInnovatorTest.XUnit.AlphabeticalOrderer", "WInnovatorTest")]
    public class DesignShopControllerTest : IClassFixture<DesignShopControllerTestFixture>
    {
        private DesignShopControllerTestFixture _fixture;

        public DesignShopControllerTest(DesignShopControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test1_DesignShopCreate()
        {
            // Act
            var result = await _fixture._controller.CreateDesignShop();

            // Assert
            // Peel off seperate layers
            var firstResult = Assert.IsType<ActionResult<DesignShop>>(result);
            var secondResult = Assert.IsType<CreatedAtActionResult>(firstResult.Result);
            // Finally we get the desired value, cast it to use it
            _fixture._createdDesignShop = (DesignShop)secondResult.Value;
            // Assert that we got an ID
            Assert.False(string.IsNullOrEmpty(_fixture._createdDesignShop.Id.ToString()));

        }

        [Fact]
        public async Task Test2_GetListOfPresentDesignShops()
        {
            // Act
            // Task<ActionResult<IEnumerable<DesignShopViewModel>>> GetDesignShop
            var result = await _fixture._controller.GetDesignShop();

            // Assert
            // Peel off seperate layers and test if they're the correct type
            // We should have a IEnumerable with 1 item
            var firstResult = Assert.IsType<ActionResult<IEnumerable<DesignShopViewModel>>>(result);
            List<DesignShopViewModel> listOfDesignShops = Assert.IsType<List<DesignShopViewModel>>(firstResult.Value);

            // Assert that we've got one (1) item
            Assert.True(listOfDesignShops.Count == 1);
            // Assert that we got the ID of the design shop that's made in the first test
            Assert.True(listOfDesignShops[0].Id == _fixture._createdDesignShop.Id);

        }

        [Fact]
        public async Task Test3_GetQrCodeForDesignShopCreatedInFirstTest_GetFileStreamResult()
        {
            // Act
            var qrcodeFirstResult = await _fixture._controller.GetQrCode(_fixture._createdDesignShop.Id);

            // Assert
            Assert.IsType<FileStreamResult>(qrcodeFirstResult);
        }

        [Fact]
        public async Task Test4_CallGetDesignShopWithUnknownGuid_ExpectNotFoundResult()
        {
            // Act
            var result = await _fixture._controller.GetDesignShop(new System.Guid());

            // Assert
            var firstResult = Assert.IsType<ActionResult<DesignShopViewModel>>(result);
            var secondResult = Assert.IsType<NotFoundResult>(firstResult.Result);
        }
    }
}
