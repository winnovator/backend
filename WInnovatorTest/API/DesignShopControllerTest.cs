using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var firstResult = Assert.IsType<ActionResult<List<DesignShopViewModel>>>(result);
            List<DesignShopViewModel> listOfDesignShops = Assert.IsType<List<DesignShopViewModel>>(firstResult.Value);

            // Assert that we've got 3 items
            Assert.True(listOfDesignShops.Count == 3);
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
            var qrcodeFirstResult = _fixture._controller.GetQrCode(Guid.NewGuid());

            // Assert
            Assert.IsType<NotFoundResult>(qrcodeFirstResult);
        }

        [Fact]
        public async Task Test4_GetAppTokenWithValidDesignShopId()
        {
            // Act
            var result = await _fixture._controller.GetAppToken(_fixture._designShop.Id);
            
            // Asserts
            var firstResult = Assert.IsType<ActionResult<AppTokenModel>>(result);
            var secondResult = Assert.IsType<OkObjectResult>(firstResult.Result);
            var finalResult = secondResult.Value;
            Assert.IsType<AppTokenModel>(finalResult);
            AppTokenModel appTokenModel = (AppTokenModel) finalResult;
            Assert.True(appTokenModel.ShopId.Equals(_fixture._designShop.Id));
            Assert.True(appTokenModel.ShopDescription.Equals(_fixture._designShop.Description));
            Assert.True(appTokenModel.Token.Equals(_fixture._designShop.AppUseraccount));
        }

        [Fact]
        public async Task Test5_GetAppTokenWithInvalidDesignShopId()
        {
            // Act
            var result = await _fixture._controller.GetAppToken(Guid.NewGuid());
            
            // Asserts
            var firstResult = Assert.IsType<ActionResult<AppTokenModel>>(result);
            var secondResult = Assert.IsType<NotFoundResult>(firstResult.Result);
        }
    }
}