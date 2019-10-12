using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WInnovator.Models;
using WInnovatorTest.API.Fixtures;
using Xunit;

namespace WInnovatorTest.API
{
    [TestCaseOrderer("WInnovatorTest.XUnit.AlphabeticalOrderer", "WInnovatorTest")]
    public class DownloadImageControllerTest : IClassFixture<DownloadImageControllerTestFixture>
    {
        private DownloadImageControllerTestFixture _fixture;

        public DownloadImageControllerTest(DownloadImageControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test1_DownloadImageWithValidId()
        {
            // Act
            var result = await _fixture._controller.GetImage(_fixture._imageStore.Id);

            // Assert
            // First assert: did we get an actionresult with an ImageStore
            var firstResult = Assert.IsType<ActionResult<ImageStore>>(result);
            // Second assert, is there a file content result
            var secondResult = Assert.IsType<FileContentResult>(result.Result);
            Assert.Equal("application/test", secondResult.ContentType);
        }

        [Fact]
        public async Task Test1_DownloadImageWithInvalidId()
        {
            // Act
            var result = await _fixture._controller.GetImage(new Guid());

            // Assert
            // First assert: did we get an actionresult with an ImageStore
            var firstResult = Assert.IsType<ActionResult<ImageStore>>(result);
            // Second assert, is there a file content result
            Assert.IsType<NotFoundResult>(result.Result);
        }
    }
}