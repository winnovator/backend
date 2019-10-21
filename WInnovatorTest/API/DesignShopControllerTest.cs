using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.ViewModels;
using WInnovatorTest.API.Fixtures;
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
        public async Task Test1_GetListOfPresentDesignShops()
        {
            // Act
            var result = await _fixture._controller.GetDesignShop();

            // Assert
            // Peel off seperate layers and test if they're the correct type
            // We should have a IEnumerable with 1 item
            var firstResult = Assert.IsType<ActionResult<IEnumerable<DesignShopViewModel>>>(result);
            List<DesignShopViewModel> listOfDesignShops = Assert.IsType<List<DesignShopViewModel>>(firstResult.Value);

            // Assert that we've got one (1) item
            Assert.True(listOfDesignShops.Count == 13);
            // Assert that we got the ID of the design shop that's made in the first test
            Assert.True(listOfDesignShops.Count(shop => shop.Id == _fixture._designShop.Id) == 1);
            Assert.True(listOfDesignShops.Count(shop => shop.Description == _fixture._designShop.Description) == 1);
            Assert.True(listOfDesignShops.Count(shop => shop.Date == _fixture._designShop.Date) == 1);
        }

        [Fact]
        public void Test2_GetQrCodeForValidDesignShop()
        {
            // Act
            var qrcodeFirstResult = _fixture._controller.GetQrCode(_fixture._designShop.Id);

            // Assert
            Assert.IsType<FileStreamResult>(qrcodeFirstResult);
        }

        [Fact]
        public void Test3_GetQrCodeForInvalidDesignShop()
        {
            // Act
            var qrcodeFirstResult = _fixture._controller.GetQrCode(new Guid());

            // Assert
            Assert.IsType<NotFoundResult>(qrcodeFirstResult);
        }
    }
}