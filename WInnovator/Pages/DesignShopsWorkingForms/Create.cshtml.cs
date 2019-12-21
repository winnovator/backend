using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WInnovator.Models;

namespace WInnovator.Pages.DesignShopsWorkingForms
{
    [ExcludeFromCodeCoverage]
    [Authorize(Roles = "Administrator,Facilitator")]
    public class CreateModel : PageModel
    {
        private readonly WInnovator.DAL.ApplicationDbContext _context;
        public SelectList WorkingForms { get; set; }
        public Guid? currentWorkingFormId { get; set; }
        [BindProperty]
        public IEnumerable<SelectListItem> CurrentWorkingForm { get; set; }
        [BindProperty]
        public DesignShopWorkingForm DesignShopWorkingForm { get; set; }
        public Guid currentDesignShop { get; set; }

        public CreateModel(WInnovator.DAL.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(Guid designshopId)
        {
            currentDesignShop = designshopId;
            // Only the current designshop can be selected, no changes can be made!
            //ViewData["DesignShopId"] = new SelectList(_context.DesignShop.Where(ds => ds.Id == designshopId), "Id", "Description");
            //WorkingForms = new SelectList(_context.WorkingForm, "Id", "Description");

            LoadItems();
            DesignShopWorkingForm = new DesignShopWorkingForm();
            //DesignShopWorkingForm.TimeAllocated = 1;

            if (WorkingForms.Count() > 0)
            {
                Guid guid = Guid.Parse(WorkingForms.ElementAt(0).Value);
                WorkingForm workingForm = _context.WorkingForm.Where(wf => wf.Id == guid).FirstOrDefault();
                if (workingForm != null)
                {
                    DesignShopWorkingForm.DisplayName = workingForm.DisplayName;
                    DesignShopWorkingForm.TimeAllocated = workingForm.DefaultTimeNeeded;
                    DesignShopWorkingForm.PhaseId = workingForm.PhaseId;
                    DesignShopWorkingForm.Resume = workingForm.Resume;
                    DesignShopWorkingForm.Description = workingForm.Description;
                }
            }
            return Page();
        }

        // Gets called when the user selects a workingform
        public IActionResult OnPost()
        {
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            try
            {
                currentDesignShop = Guid.Parse(ModelState["DesignShopWorkingForm.DesignShopId"].AttemptedValue);
                currentWorkingFormId = Guid.Parse(ModelState["CurrentWorkingForm"].AttemptedValue);
                WorkingForm currentWorkingForm = _context.WorkingForm.Where(wf => wf.Id == currentWorkingFormId).FirstOrDefault();
                if (currentWorkingForm != null)
                {
                    // Set the content from the workingform as the content for this designshopworkingform
                    ModelState.SetModelValue("DesignShopWorkingForm.DisplayName", new ValueProviderResult(currentWorkingForm.DisplayName, CultureInfo.InvariantCulture));
                    ModelState.SetModelValue("DesignShopWorkingForm.TimeAllocated", new ValueProviderResult(currentWorkingForm.DefaultTimeNeeded.ToString(), CultureInfo.InvariantCulture));
                    ModelState.SetModelValue("DesignShopWorkingForm.PhaseId", new ValueProviderResult(currentWorkingForm.PhaseId.ToString(), CultureInfo.InvariantCulture));
                    ModelState.SetModelValue("DesignShopWorkingForm.Resume", new ValueProviderResult(currentWorkingForm.Resume, CultureInfo.InvariantCulture));
                    ModelState.SetModelValue("DesignShopWorkingForm.Description", new ValueProviderResult(currentWorkingForm.Description, CultureInfo.InvariantCulture));
                }
                else
                {
                    // This normally can't happen, but just be very sure
                    currentWorkingFormId = null;
                }

                LoadItems();

                return Page();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostCreateAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Sets the selected workingform
            DesignShopWorkingForm.WorkingFormId = Guid.Parse(ModelState["CurrentWorkingForm"].AttemptedValue);

            // Get the order to put it in
            DesignShopWorkingForm.Order = getCorrectFirstPosition(DesignShopWorkingForm.DesignShopId);

            _context.DesignShopWorkingForm.Add(DesignShopWorkingForm);
            await _context.SaveChangesAsync();

            TempData["selectedDesignShop"] = DesignShopWorkingForm.DesignShopId;
            return RedirectToPage("./Index");
        }

        private void LoadItems()
        {
            // Only the current designshop can be selected, no changes can be made!
            ViewData["DesignShopId"] = new SelectList(_context.DesignShop.Where(ds => ds.Id == currentDesignShop), "Id", "Description");
            ViewData["PhaseId"] = new SelectList(_context.Phase, "Id", "Name");
            if (currentWorkingFormId != null)
            {
                WorkingForms = new SelectList(_context.WorkingForm, "Id", "Name", currentWorkingFormId);
            }
            else
            {
                WorkingForms = new SelectList(_context.WorkingForm, "Id", "Name");
            }
        }

        private int getCorrectFirstPosition(Guid designShopId)
        {
            // The first position is equal to the highest order + 1, or just 1 if no other workingform is existing
            var designshop = _context.DesignShopWorkingForm.Where(dswf => dswf.DesignShopId == designShopId).OrderByDescending(dswf => dswf.Order).FirstOrDefault();
            if (designshop == null)
            {
                return 1;
            }
            return designshop.Order + 1;
        }
    }
}
