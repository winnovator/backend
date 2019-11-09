using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WInnovator.Data;
using WInnovator.ViewModels;

namespace WInnovator.API
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class TokenController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        [ExcludeFromCodeCoverage]
        public TokenController(ILogger<TokenController> logger, ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateToken([FromBody] TokenViewModel tokenViewModel)
        {
            if(await isValidUsernameAndPassword(tokenViewModel.Email, tokenViewModel.Password)) { 
                return new ObjectResult(await GenerateToken(tokenViewModel.Email));
            } else
            {
                return BadRequest();
            }
        }

        private async Task<bool> isValidUsernameAndPassword(string username, string password)
        {
            var user = await _userManager.FindByEmailAsync(username);
            return await _userManager.CheckPasswordAsync(user, password);
        }

        private async Task<dynamic> GenerateToken(string username)
        {
            // Source: https://www.youtube.com/watch?v=9QU_y7-VsC8
            var user = await _userManager.FindByEmailAsync(username);
            var roles = from ur in _context.UserRoles
                        join r in _context.Roles on ur.RoleId equals r.Id
                        where ur.UserId == user.Id
                        select new { ur.UserId, ur.RoleId, r.Name };

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),                // immediately valid
                new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())      // When expiring
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));
            }

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])),
                                SecurityAlgorithms.HmacSha256)),
                                new JwtPayload(claims));

            var output = new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                username
            };

            return output;
        }
    }
}