using Microsoft.AspNetCore.Http;
using Moq;
using System.IO;

namespace WInnovatorTest.TestHelper
{
    public static class MockFile
    {
        public static IFormFile mock(string fileName, string contentType)
        {
            //Arrange
            var fileMock = new Mock<IFormFile>();
            //Setup mock file using a memory stream
            var content = "Hello World from a Fake File";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.ContentType).Returns(contentType);
            return fileMock.Object;
        }
    }
}