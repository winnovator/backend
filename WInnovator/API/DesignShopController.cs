using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Data;
using WInnovator.Models;
using WInnovator.ViewModels;

namespace WInnovator.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class DesignShopController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DesignShopController> _logger;

        public DesignShopController(ApplicationDbContext context, ILogger<DesignShopController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/DesignShop
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DesignShopViewModel>>> GetDesignShop()
        {
            IEnumerable<DesignShop> list = await _context.DesignShop.Include(shops => shops.UploadedImages).ToListAsync();
            IEnumerable<DesignShopViewModel> shopList = list.Select(shop => new DesignShopViewModel { Id = shop.Id, numberOfUploadedImages = shop.UploadedImages.Count });
            return shopList.ToList();
        }

        // GET: api/DesignShop/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DesignShopViewModel>> GetDesignShop(Guid id)
        {
            if (!DesignShopExists(id))
            {
                return NotFound();
            }

            var designShop = await _context.DesignShop.Include(shop => shop.UploadedImages).FirstOrDefaultAsync(shop => shop.Id == id);

            return new DesignShopViewModel { Id=designShop.Id, numberOfUploadedImages=designShop.UploadedImages.Count };
        }

        /// <summary>
        /// Gets a QrCode for the requested guid
        /// </summary>
        /// <returns>An image containing the specified QrCode</returns>
        [HttpGet("{id}/qrcode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetQrCode(Guid id)
        {
            if (!DesignShopExists(id))
            {
                return NotFound();
            }
            
            var designShop = await _context.DesignShop.Include(shop => shop.UploadedImages).FirstOrDefaultAsync(shop => shop.Id == id);

            try
            {
                var qrCode = createQrCode(id);
                var outputStream = new MemoryStream();
                qrCode.Save(outputStream, ImageFormat.Jpeg);
                outputStream.Seek(0, SeekOrigin.Begin);
                _logger.LogTrace($"QrCode for guid { id } being served");
                return File(outputStream, "image/jpeg");
            }
            catch
            {
                _logger.LogError("OOPS... something went wrong!!");
                return NotFound();
            }
        }


        // PUT: api/DesignShop/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutDesignShop(Guid id, DesignShop designShop)
        //{
        //    if (id != designShop.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(designShop).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!DesignShopExists(id))
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

        // POST: api/DesignShop
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("create")]
        public async Task<ActionResult<DesignShop>> CreateDesignShop()
        {
            // Currently, we only need an empty DesignShop so we'll create it here.
            _logger.LogTrace("Creating new Design Shop.");
            DesignShop designShop = new DesignShop();
            _context.DesignShop.Add(designShop);
            await _context.SaveChangesAsync();
            _logger.LogTrace($"New Design Shop created with id { designShop.Id }");

            return CreatedAtAction("GetDesignShop", new { id = designShop.Id }, designShop);
        }

        // DELETE: api/DesignShop/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<DesignShop>> DeleteDesignShop(Guid id)
        //{
        //    var designShop = await _context.DesignShop.FindAsync(id);
        //    if (designShop == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.DesignShop.Remove(designShop);
        //    await _context.SaveChangesAsync();

        //    return designShop;
        //}

        private bool DesignShopExists(Guid id)
        {
            return _context.DesignShop.Any(e => e.Id == id);
        }

        private Bitmap createQrCode(Guid guid)
        {
            Color darkColor = ColorTranslator.FromHtml("#000000");
            Color lightColor = ColorTranslator.FromHtml("#ffffff");
            Bitmap icon = new Bitmap(WInnovator.Properties.Resources.WInnovator_wit);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(guid.ToString(), QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20, darkColor, lightColor, icon, 25, 20);
        }

    }
}
