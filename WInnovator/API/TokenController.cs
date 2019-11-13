using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WInnovator.Interfaces;
using WInnovator.ViewModels;

namespace WInnovator.API
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IUserIdentityHelper _userIdentityHelper;

        [ExcludeFromCodeCoverage]
        public TokenController(IUserIdentityHelper userIdentityHelper)
        {
            _userIdentityHelper = userIdentityHelper;
        }

        /// <summary>
        /// Gets a token for the requested user
        /// </summary>
        /// <param name="tokenViewModel">TokenViewModel with username and password</param>
        /// <returns>A token if the user is valid</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TokenModel>> GenerateToken([FromBody] TokenViewModel tokenViewModel)
        {
            if(await _userIdentityHelper.CredentialsAreValid(tokenViewModel.Email, tokenViewModel.Password)) { 
                return Ok(await GenerateToken(tokenViewModel.Email));
            } else
            {
                return BadRequest();
            }
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            return new TokenModel(await _userIdentityHelper.GenerateJwtToken(username), username);
        }
    }
}