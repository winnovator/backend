using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WInnovator.Data;
using WInnovator.Interfaces;
using WInnovator.ViewModels;

namespace WInnovator.API
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IJwtTokenService _tokenService;
        private readonly ILogger<TokenController> _logger;

        [ExcludeFromCodeCoverage]
        public TokenController(IJwtTokenService tokenService, ILogger<TokenController> logger)
        {
            _tokenService = tokenService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult GenerateToken([FromBody] TokenViewModel tokenViewModel)
        {
            var token = _tokenService.BuildToken(tokenViewModel.Email);
            return Ok(new {token});
        }

    }
}