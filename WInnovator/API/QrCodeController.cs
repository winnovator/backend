using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace WInnovator.API
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class QrCodeController : ControllerBase
    {
        private ILogger<QrCodeController> _logger;

        public QrCodeController(ILogger<QrCodeController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Gets a default QrCode
        /// </summary>
        /// <returns>An image containing the specified QrCode</returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            try
            {
                var qrCode = createQrCode();
                var outputStream = new MemoryStream();
                qrCode.Save(outputStream, ImageFormat.Jpeg);
                outputStream.Seek(0, SeekOrigin.Begin);
                _logger.LogInformation("QrCode being served");
                return File(outputStream, "image/jpeg");
            }
            catch
            {
                _logger.LogError("OOPS... something went wrong!!");
                return NotFound();
            }
        }

        private Bitmap createQrCode()
        {
            Color darkColor = ColorTranslator.FromHtml("#000000");
            Color lightColor = ColorTranslator.FromHtml("#ffffff");
            Bitmap icon = new Bitmap(WInnovator.Properties.Resources.WInnovator_wit);

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode("https://www.windesheim.nl/", QRCodeGenerator.ECCLevel.H);
            QRCode qrCode = new QRCode(qrCodeData);
            return qrCode.GetGraphic(150, darkColor, lightColor, icon, 25, 20);

        }
    }
}