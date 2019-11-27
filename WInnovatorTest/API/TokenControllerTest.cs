using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using WInnovator.API;
using WInnovator.Interfaces;
using WInnovator.ViewModels;
using Xunit;

namespace WInnovatorTest.API
{
    [TestCaseOrderer("WInnovatorTest.XUnit.AlphabeticalOrderer", "WInnovatorTest")]
    public class TokenControllerTest
    {
        private Mock<IUserIdentityHelper> _userIdentityHelper;
        private TokenController _controller;

        public TokenControllerTest()
        {
            // Arrange
            _userIdentityHelper = new Mock<IUserIdentityHelper>();
            _userIdentityHelper.Setup(uih => uih.CredentialsAreValid("Valid", "Valid")).ReturnsAsync(true);
            _userIdentityHelper.Setup(uih => uih.CredentialsAreValid(It.IsAny<string>(), "Invalid")).ReturnsAsync(false);
            _userIdentityHelper.Setup(uih => uih.CredentialsAreValid("Invalid", It.IsAny<string>())).ReturnsAsync(false);
            _userIdentityHelper.Setup(uih => uih.GenerateJwtToken(It.IsAny<string>())).ReturnsAsync((string s) => s + "token");
            _controller = new TokenController( _userIdentityHelper.Object);
        }

        [Fact]
        public async Task Test1_GetTokenWithValidUsernameAndPassword()
        {
            // Arrange
            TokenViewModel tvm = new TokenViewModel() { Email = "Valid", Password = "Valid" };
            
            // Act
            var result = await _controller.GenerateToken(tvm);
            
            // Assert
            var firstResult = Assert.IsType<ActionResult<TokenModel>>(result);
            var secondResult = Assert.IsType<OkObjectResult>(firstResult.Result);
            var finalResult = secondResult.Value;
            Assert.IsType<TokenModel>(finalResult);
            
            TokenModel tokenModel = (TokenModel) finalResult;
            Assert.True(tokenModel.Token.Equals("Validtoken"));
            Assert.True(tokenModel.Username.Equals("Valid"));
        }
        
        [Fact]
        public async Task Test2_GetTokenWithValidUsernameAndInvalidPassword()
        {
            // Arrange
            TokenViewModel tvm = new TokenViewModel() { Email = "Valid", Password = "Invalid" };
            
            // Act
            var result = await _controller.GenerateToken(tvm);
            
            // Assert
            var firstResult = Assert.IsType<ActionResult<TokenModel>>(result);
            var secondResult = Assert.IsType<BadRequestResult>(firstResult.Result);
        }

        [Fact]
        public async Task Test3_GetTokenWithInvalidUsername()
        {
            // Arrange
            TokenViewModel tvm = new TokenViewModel() { Email = "Invalid", Password = "justapassword" };
            
            // Act
            var result = await _controller.GenerateToken(tvm);
            
            // Assert
            var firstResult = Assert.IsType<ActionResult<TokenModel>>(result);
            var secondResult = Assert.IsType<BadRequestResult>(firstResult.Result);
        }
    }
}