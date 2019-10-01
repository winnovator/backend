using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using WInnovator.API;
using Xunit;

namespace WInnovatorTest
{
    public class Tests
    {
        [Fact]
        public void TestQrCode_GetOK()
        {
            // Arrange
            ILogger<QrCodeController> logger = Mock.Of<ILogger<QrCodeController>>();
            var controller = new QrCodeController(logger);

            // Act
            var result = controller.Get();

            //Assert
            Assert.IsType<FileStreamResult>(result);
        }
    }
}