using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Data;
using WInnovator.Models;
using WInnovator.Properties;
using WInnovator.ViewModels;

namespace WInnovator.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class DesignShopController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DesignShopController> _logger;

        [ExcludeFromCodeCoverage]
        public DesignShopController(ApplicationDbContext context, ILogger<DesignShopController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Generates a list of designshops, starting at the current date or in the future
        /// </summary>
        /// <returns>A list of designshops</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<DesignShopViewModel>>> GetDesignShop()
        {
            IEnumerable<DesignShop> list = await _context.DesignShop
                .Where(shop => DateTime.Now.Date <= shop.Date.Date)
                .OrderBy(shop => shop.Date)
                .ToListAsync();

            IEnumerable<DesignShopViewModel> shopList = list.Select(shop => new DesignShopViewModel
                {Id = shop.Id, Description = shop.Description, Date = shop.Date});
            return shopList.ToList();
        }

        /// <summary>
        /// Gets a QrCode for the requested guid
        /// </summary>
        /// <returns>An image containing the specified QrCode</returns>
        [HttpGet("{id}/qrcode")]
        [Authorize(Roles = "Administrator,Facilitator")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetQrCode(Guid id)
        {
            if (!DesignShopExists(id))
            {
                return NotFound();
            }

            var qrCode = createQrCode(id);
            var outputStream = new MemoryStream();
            qrCode.Save(outputStream, ImageFormat.Jpeg);
            outputStream.Seek(0, SeekOrigin.Begin);
            _logger.LogTrace($"QrCode for guid {id} being served");
            return File(outputStream, "image/jpeg");
        }

        private bool DesignShopExists(Guid id)
        {
            return _context.DesignShop.Any(e => e.Id == id);
        }

        [ExcludeFromCodeCoverage]
        private Bitmap createQrCode(Guid guid)
        {
            Color darkColor = ColorTranslator.FromHtml("#000000");
            Color lightColor = ColorTranslator.FromHtml("#ffffff");
            Bitmap icon = new Bitmap(Resources.WInnovator_logo);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(guid.ToString(), QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(20, darkColor, lightColor, icon, 25, 20);
        }
    }
}