using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WInnovator.Models;
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
        public async Task Test2_GetQrCodeForDesignShopCreatedInFirstTest_GetFileStreamResult()
        {
            // Act
            var qrcodeFirstResult = await _fixture._controller.GetQrCode(_fixture._createdDesignShop.Id);

            //Assert
            Assert.IsType<FileStreamResult>(qrcodeFirstResult);
        }
    }
}
