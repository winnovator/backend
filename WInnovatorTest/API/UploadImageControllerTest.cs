using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WInnovator.Models;
using WInnovatorTest.API.Fixtures;
using WInnovatorTest.TestHelper;
using Xunit;

namespace WInnovatorTest.API
{
    [TestCaseOrderer("WInnovatorTest.XUnit.AlphabeticalOrderer", "WInnovatorTest")]
    public class UploadImageControllerTest : IClassFixture<UploadImageControllerTestFixture>
    {
        private UploadImageControllerTestFixture _fixture;

        public UploadImageControllerTest(UploadImageControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test1_UploadToDesignShopWithValidCurrentWorkingForm()
        {
            // Act
            var result = await _fixture._controller.PostUploadImageStore(_fixture._designShopWithCurrentWorkingForm.Id,
                MockFile.mock("example1.png", "image/png"));

            // Assert
            // First assert: did we get an actionresult with an ImageStore
            var firstResult = Assert.IsType<ActionResult<ImageStore>>(result);
            // Second assert, is the file accepted
            Assert.IsType<AcceptedResult>(result.Result);
        }

        [Fact]
        public async Task Test2_UploadToDesignShopWithoutValidCurrentWorkingForm()
        {
            // Act
            var result = await _fixture._controller.PostUploadImageStore(
                _fixture._designShopWithoutCurrentWorkingForm.Id, MockFile.mock("example.png", "image/png"));

            // Assert
            // First assert: did we get an actionresult with an ImageStore
            var firstResult = Assert.IsType<ActionResult<ImageStore>>(result);
            // Second assert, is the file accepted
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Test3_UploadToDesignShopWithValidCurrentWorkingFormButInvalidFileExtension()
        {
            // Act
            var result = await _fixture._controller.PostUploadImageStore(_fixture._designShopWithCurrentWorkingForm.Id,
                MockFile.mock("example1.pdf", "image/png"));

            // Assert
            // First assert: did we get an actionresult with an ImageStore
            var firstResult = Assert.IsType<ActionResult<ImageStore>>(result);
            // Second assert, is the file accepted
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task Test4_UploadToDesignShopWithValidCurrentWorkingFormButInvalidMimeType()
        {
            // Act
            var result = await _fixture._controller.PostUploadImageStore(_fixture._designShopWithCurrentWorkingForm.Id,
                MockFile.mock("example1.png", "application/pdf"));

            // Assert
            // First assert: did we get an actionresult with an ImageStore
            var firstResult = Assert.IsType<ActionResult<ImageStore>>(result);
            // Second assert, is the file accepted
            Assert.IsType<BadRequestResult>(result.Result);
        }
    }
}