using Microsoft.AspNetCore.Mvc;
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
            var controller = new QrCodeController();

            // Act
            var result = controller.Get();

            //Assert
            Assert.IsType<FileStreamResult>(result);
        }
    }
}