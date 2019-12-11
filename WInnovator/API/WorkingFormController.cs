using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WInnovator.Data;
using WInnovator.Models;
using WInnovator.ViewModels;

namespace WInnovator.API
{
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Facilitator")]
        public async Task<ActionResult<List<WorkingFormViewModel>>> GetListOfWorkingForms(Guid designShopId)
        {
            // Check if designshop exists
            if (!DesignShopExists(designShopId))
            {
                _logger.LogWarning($"Unknown designshop, asked for id {designShopId}");
                return NotFound();
            }

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Facilitator")]
        public async Task<ActionResult<WorkingFormViewModel>> GetCurrentWorkingFormOfDesignShop(Guid designShopId)
        {
            // Check if designshop exists
            if (!DesignShopExists(designShopId))
            {
                _logger.LogWarning($"Unknown designshop, asked for id {designShopId}");
                return NotFound();
            }

            _logger.LogTrace($"Searching current workingform for designshop id {designShopId}.");

            DesignShopWorkingForm dswf = await getCurrentWorkingFormForDesignShop(designShopId);

            var currentWorkingForm = new WorkingFormViewModel();
            if (dswf == null)
            {
                _logger.LogWarning($"Designshop with id {designShopId} doesn't have a current workingform.'");
                return NotFound();
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
        /// Sets the next workingform of the specified designshop
        /// </summary>
        /// <param name="designShopId">guid of the designshop</param>
        /// <returns>WorkingFormModelView of the next workingform</returns>
        [HttpGet("{designShopId}/next")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Facilitator")]
        public async Task<ActionResult<WorkingFormViewModel>> SetNextWorkingFormOfDesignShop(Guid designShopId)
        {
            int currentInOrder = 0;

            // Check if designshop exists
            if (!DesignShopExists(designShopId))
            {
                _logger.LogWarning($"Unknown designshop, asked for id {designShopId}");
                return NotFound();
            }

            _logger.LogTrace($"Searching current workingform for designshop id {designShopId}.");

            DesignShopWorkingForm dswf = await getCurrentWorkingFormForDesignShop(designShopId);

            if (dswf != null)
            {
                // Remember ordernumber of current workingform and update it so it isn't current anymore
                currentInOrder = dswf.Order;
                dswf.IsCurrentWorkingForm = false;
                _context.DesignShopWorkingForm.Update(dswf);
                await _context.SaveChangesAsync();
            }

            // Get the next
            dswf = await _context.DesignShopWorkingForm
                .Where(dswf => dswf.DesignShopId == designShopId && dswf.Order > currentInOrder)
                .Include(dswf => dswf.WorkingForm)
                .OrderBy(dswf => dswf.Order)
                .FirstOrDefaultAsync();

            if(dswf != null)
            {
                // Set the found workingform as current
                dswf.IsCurrentWorkingForm = true;
                _context.DesignShopWorkingForm.Update(dswf);
                await _context.SaveChangesAsync();
            }

            var currentWorkingForm = new WorkingFormViewModel();
            if (dswf == null)
            {
                _logger.LogWarning($"Designshop with id {designShopId} doesn't have a current workingform.'");
                return NotFound();
            }
            else
            {
                currentWorkingForm = new WorkingFormViewModel()
                { Id = dswf.Id, Description = dswf.WorkingForm.Description };
                _logger.LogTrace(
                    $"Designshop with id {designShopId} now has workingform with id {currentWorkingForm.Id} as active workingform.");
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator,Facilitator")]
        public async Task<ActionResult<List<DownloadImageViewModel>>> GetListOfImagesOfWorkingForm(Guid workingFormId)
        {
            // Check if the DesignShopWorkingForm exists
            if (!DesignShopWorkingFormExists(workingFormId))
            {
                _logger.LogWarning($"Unknown DesignShopWorkingForm, asked for id {workingFormId}");
                return NotFound();
            }

            DesignShopWorkingForm designShopWorkingForm = await _context.DesignShopWorkingForm
                .Where(dswf => dswf.Id == workingFormId)
                .Include(dswf => dswf.UploadedImages)
                .FirstOrDefaultAsync();

            var listOfImages = designShopWorkingForm.UploadedImages.Select(image => new DownloadImageViewModel()
                {Id = image.Id, DateTime = image.UploadDateTime}).ToList();

            _logger.LogTrace($"Found {listOfImages.Count} images for DesignShopWorkingForm with id {workingFormId}");
            return listOfImages;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("reorder")]
        [Authorize(Roles = "Administrator,Facilitator")]
        public ActionResult ChangeOrderOfWorkingForms([FromForm]String itemIds)
        {
            int count = 1;
            List<Guid> itemList = itemIds.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(s => Guid.Parse(s)).ToList();
            foreach (Guid id in itemList)
            {
                try
                {
                    DesignShopWorkingForm form = _context.DesignShopWorkingForm.Where(dswf => dswf.Id == id).FirstOrDefault();
                    form.Order = count;
                    _context.DesignShopWorkingForm.Update(form);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    _logger.LogError($"An error occurred while changing the order of DesignShopWorkingForm { id }. Stacktrace:\n" + e.StackTrace.ToString());
                    continue;
                }
                count++;
            }
            return Ok();
        }

        private async Task<DesignShopWorkingForm> getCurrentWorkingFormForDesignShop(Guid designShopId)
        {
            return await _context.DesignShopWorkingForm
                .Where(dswf => dswf.DesignShopId == designShopId && dswf.IsCurrentWorkingForm == true)
                .Include(dswf => dswf.WorkingForm)
                .FirstOrDefaultAsync();
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