using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Text;
using WInnovator.API;
using WInnovator.Models;
using WInnovatorTest.Data;

namespace WInnovatorTest.API.Fixtures
{
    public class DownloadImageControllerTestFixture : DbContextTest
    {
        public ILogger<DownloadImageController> _logger;
        public DownloadImageController _controller;
        public ImageStore _imageStore;

        public DownloadImageControllerTestFixture()
        {
            // Arrange
            _logger = Mock.Of<ILogger<DownloadImageController>>();
            _controller = new DownloadImageController(_applicationTestDbContext, _logger);
            MockImages();
        }

        private void MockImages()
        {
            _imageStore = new ImageStore()
            {
                Id = new Guid(),
                Image = Encoding.ASCII.GetBytes("TestImage"),
                UploadDateTime = DateTime.Now,
                Mimetype = "application/test"
            };
            _applicationTestDbContext.ImageStore.Add(_imageStore);
            _applicationTestDbContext.SaveChanges();
        }
    }
}