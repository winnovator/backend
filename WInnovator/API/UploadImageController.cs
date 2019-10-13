using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WInnovator.Data;
using WInnovator.Models;

namespace WInnovator.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UploadImageController> _logger;

        private readonly List<string> acceptedImageMimetypes;
        private readonly List<string> acceptedImageExtensions;

        [ExcludeFromCodeCoverage]
        public UploadImageController(ApplicationDbContext context, ILogger<UploadImageController> logger)
        {
            _context = context;
            _logger = logger;

            acceptedImageMimetypes = new List<string>()
                {"image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png"};
            acceptedImageExtensions = new List<string>() {".jpg", ".png", ".gif", ".jpeg"};
        }

        /// <summary>
        /// Upload an image
        /// </summary>
        /// <param name="designShopId">The designshop to which the image belongs</param>
        /// <param name="uploadedFile">The image itself</param>
        /// <returns>The correct statuscode (202, 400 or 404)</returns>
        [HttpPost("{designShopId}")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ImageStore>> PostUploadImageStore(Guid designShopId, [FromForm] IFormFile uploadedFile)
        {
            if (uploadedFile == null)
            {
                return BadRequest();
            }

            // Check if the DesignShop has a current WorkingForm set
            var currentShop = await _context.DesignShop.Include(shop => shop.CurrentWorkingForm)
                .FirstOrDefaultAsync(shop => shop.Id == designShopId);
            
            if (currentShop.CurrentWorkingForm == null)
            {
                // There's no workingform set, return 404 Not Found
                return NotFound();
            }

            if (!contentHasImageMimetype(uploadedFile.ContentType) ||
                !contentHasImageExtension(Path.GetExtension(uploadedFile.FileName)))
            {
                return BadRequest();
            }

            var imageStore = new ImageStore
            {
                DesignShopWorkingForm = currentShop.CurrentWorkingForm,
                Mimetype = uploadedFile.ContentType,
                UploadDateTime = DateTime.Now
            };

            await using (var memoryStream = new MemoryStream())
            {
                await uploadedFile.CopyToAsync(memoryStream);
                imageStore.Image = memoryStream.ToArray();
            }

            _context.ImageStore.Add(imageStore);
            await _context.SaveChangesAsync();

            _logger.LogTrace(
                $"Uploaded image with ID {imageStore.Id} to DesignShopWorkingForm with ID {imageStore.DesignShopWorkingForm.Id}");

            return Accepted();
        }

        private bool contentHasImageMimetype(string contentType)
        {
            return !string.IsNullOrEmpty(contentType.Trim()) && acceptedImageMimetypes.Any(mt => mt.Contains(contentType.ToLower()));
        }

        private bool contentHasImageExtension(string extension)
        {
            return !string.IsNullOrEmpty(extension.Trim()) && acceptedImageExtensions.Any(ext => ext.Contains(extension.ToLower()));
        }
    }
}