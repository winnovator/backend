using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Data;
using WInnovator.Models;

namespace WInnovator.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadImageStoreController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UploadImageStoreController> _logger;

        private readonly List<string> acceptedImageMimetypes;
        private readonly List<string> acceptedImageExtensions;

        public UploadImageStoreController(ApplicationDbContext context, ILogger<UploadImageStoreController> logger)
        {
            _context = context;
            _logger = logger;

            acceptedImageMimetypes = new List<string>() { "image/jpg", "image/jpeg", "image/pjpeg", "image/gif", "image/x-png", "image/png" };
            acceptedImageExtensions = new List<string>() { ".jpg", ".png", ".gif", ".jpeg" };
        }

        // GET: api/UploadImageStore
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UploadImageStore>>> GetUploadImageStore()
        {
            return await _context.UploadImageStore.ToListAsync();
        }

        // GET: api/UploadImageStore/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UploadImageStore>> GetUploadImageStore(Guid id)
        {
            var uploadImageStore = await _context.UploadImageStore.FindAsync(id);

            if (uploadImageStore == null)
            {
                return NotFound();
            }

            return uploadImageStore;
        }


        // GET: api/UploadImageStore/5/Image
        [HttpGet("GetImage/{id}")]
        public async Task<ActionResult<UploadImageStore>> GetImage(Guid id)
        {
            var uploadImageStore = await _context.UploadImageStore.FindAsync(id);

            if (uploadImageStore == null)
            {
                return NotFound();
            }

            return File(uploadImageStore.UploadedImage, uploadImageStore.Mimetype);
        }


        // PUT: api/UploadImageStore/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUploadImageStore(Guid id, UploadImageStore uploadImageStore)
        //{
        //    if (id != uploadImageStore.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(uploadImageStore).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UploadImageStoreExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/UploadImageStore
        [HttpPost]
        public async Task<ActionResult<UploadImageStore>> PostUploadImageStore(Guid designShopId, IFormFile uploadedImage)
        {
            if(uploadedImage==null || designShopId == null)
            { 
                return BadRequest();
            }
            if(!contentHasImageMimetype(uploadedImage.ContentType) || !contentHasImageExtension(Path.GetExtension(uploadedImage.FileName)))
            {
                return BadRequest();
            }

            UploadImageStore uploadImageStore = new UploadImageStore();
            uploadImageStore.DesignShopId = designShopId;
            uploadImageStore.Mimetype = uploadedImage.ContentType;

            using (var memoryStream = new MemoryStream())
            {
                await uploadedImage.CopyToAsync(memoryStream);
                uploadImageStore.UploadedImage = memoryStream.ToArray();
            }

            _context.UploadImageStore.Add(uploadImageStore);
            await _context.SaveChangesAsync();

            _logger.LogTrace($"Uploaded image with ID { uploadImageStore.Id } to Design Shop with ID { uploadImageStore.DesignShopId }");

            return CreatedAtAction("GetUploadImageStore", new { id = uploadImageStore.Id }, uploadImageStore);
        }

        private bool contentHasImageMimetype(string contentType)
        {
            return acceptedImageMimetypes.Any(mt => mt.Contains(contentType.ToLower()));
        }

        private bool contentHasImageExtension(string extension)
        {
            return acceptedImageExtensions.Any(ext => ext.Contains(extension.ToLower()));
        }

        // DELETE: api/UploadImageStore/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<UploadImageStore>> DeleteUploadImageStore(Guid id)
        //{
        //    var uploadImageStore = await _context.UploadImageStore.FindAsync(id);
        //    if (uploadImageStore == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.UploadImageStore.Remove(uploadImageStore);
        //    await _context.SaveChangesAsync();

        //    return uploadImageStore;
        //}

        private bool UploadImageStoreExists(Guid id)
        {
            return _context.UploadImageStore.Any(e => e.Id == id);
        }
    }
}
