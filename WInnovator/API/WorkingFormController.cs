using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Data;
using WInnovator.Models;
using WInnovator.ViewModels;

namespace WInnovator.API
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Facilitator")]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkingFormController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WorkingFormController> _logger;

        [ExcludeFromCodeCoverage]
        public WorkingFormController(ApplicationDbContext context, ILogger<WorkingFormController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Returns a list with all the workingforms of the specified designshop.
        /// </summary>
        /// <param name="designShopId">guid of the designshop</param>
        /// <returns>A list of WorkingFormViewModels</returns>
        [HttpGet("{designShopId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<WorkingFormViewModel>>> GetListOfWorkingForms(Guid designShopId)
        {
            // Check if designshop exists
            if (!DesignShopExists(designShopId))
            {
                _logger.LogError($"Unknown designshop, asked for id {designShopId}");
                return NotFound();
            }

            _logger.LogTrace($"Searching workingforms for designshop id {designShopId}");

            IEnumerable<DesignShopWorkingForm> list = await _context.DesignShopWorkingForm
                .Where(dswf => dswf.DesignShopId == designShopId)
                .OrderBy(dswf => dswf.Order)
                .Include(dswf => dswf.WorkingForm)
                .ToListAsync();

            var listOfWorkForms = list.Select(dswf => new WorkingFormViewModel()
                {Id = dswf.Id, Description = dswf.WorkingForm.Description}).ToList();

            _logger.LogTrace($"Found {listOfWorkForms.Count} workingforms for designshop id {designShopId}");
            return listOfWorkForms;
        }

        /// <summary>
        /// Returns the current workingform of the specified designshop
        /// </summary>
        /// <param name="designShopId">guid of the designshop</param>
        /// <returns>WorkingFormModelView of the current workingform</returns>
        [HttpGet("{designShopId}/current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<WorkingFormViewModel>> GetCurrentWorkingFormOfDesignShop(Guid designShopId)
        {
            // Check if designshop exists
            if (!DesignShopExists(designShopId))
            {
                _logger.LogError($"Unknown designshop, asked for id {designShopId}");
                return NotFound();
            }

            _logger.LogTrace($"Searching current workingform for designshop id {designShopId}.");

            DesignShopWorkingForm dswf = await _context.DesignShopWorkingForm
                .Where(dswf => dswf.DesignShopId == designShopId && dswf.IsCurrentWorkingForm == true)
                .Include(dswf => dswf.WorkingForm)
                .FirstOrDefaultAsync();

            var currentWorkingForm = new WorkingFormViewModel();
            if (dswf == null)
            {
                _logger.LogTrace($"Designshop with id {designShopId} doesn't have a current workingform.'");
            }
            else
            {
                currentWorkingForm = new WorkingFormViewModel()
                    {Id = dswf.Id, Description = dswf.WorkingForm.Description};
                _logger.LogTrace(
                    $"Designshop with id {designShopId} has workingform with id {currentWorkingForm.Id} as active workingform.");
            }

            return currentWorkingForm;
        }

        /// <summary>
        /// Returns a list of all images belonging to the specified DesignShopWorkingForm
        /// </summary>
        /// <param name="workingFormId">guid of the specified DesignShopWorkingForm</param>
        /// <returns>A list of DownloadImageViewModels</returns>
        [HttpGet("{workingFormId}/imageList")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<DownloadImageViewModel>>> GetListOfImagesOfWorkingForm(Guid workingFormId)
        {
            // Check if the DesignShopWorkingForm exists
            if (!DesignShopWorkingFormExists(workingFormId))
            {
                _logger.LogError($"Unknown DesignShopWorkingForm, asked for id {workingFormId}");
                return NotFound();
            }

            _logger.LogTrace($"Searching for images belonging to DesignShopWorkingForm with id {workingFormId}.");

            DesignShopWorkingForm designShopWorkingForm = await _context.DesignShopWorkingForm
                .Where(dswf => dswf.Id == workingFormId)
                .Include(dswf => dswf.UploadedImages)
                .FirstOrDefaultAsync();

            var listOfImages = designShopWorkingForm.UploadedImages.Select(image => new DownloadImageViewModel()
                {Id = image.Id, DateTime = image.UploadDateTime}).ToList();

            _logger.LogTrace($"Found {listOfImages.Count} images for DesignShopWorkingForm with id {workingFormId}");
            return listOfImages;
        }

        private bool DesignShopExists(Guid id)
        {
            return _context.DesignShop.Any(e => e.Id == id);
        }

        private bool DesignShopWorkingFormExists(Guid id)
        {
            return _context.DesignShopWorkingForm.Any(e => e.Id == id);
        }
    }
}