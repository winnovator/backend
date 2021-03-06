using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WInnovator.DAL;
using WInnovator.Models;

namespace WInnovator.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Facilitator")]
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DownloadImageController> _logger;

        [ExcludeFromCodeCoverage]
        public DownloadImageController(ApplicationDbContext context, ILogger<DownloadImageController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Returns the image with the requested id
        /// </summary>
        /// <param name="id">guid of the image</param>
        /// <returns>The image with its original mimetype</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ImageStore>> GetImage(Guid id)
        {
            var image = await _context.ImageStore.FindAsync(id);

            if (image == null)
            {
                _logger.LogWarning($"Image with id {id} not found.");
                return NotFound();
            }

            _logger.LogTrace($"Returning image with id {id}");
            return Ok(Convert.ToBase64String(image.Image));
        }
    }
}