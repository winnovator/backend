using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WInnovator.Models;
using WInnovator.ViewModels;
using WInnovatorTest.API.Fixtures;
using Xunit;

namespace WInnovatorTest.API
{
    [TestCaseOrderer("WInnovatorTest.XUnit.AlphabeticalOrderer", "WInnovatorTest")]
    public class WorkingFormControllerTest : IClassFixture<WorkingFormControllerTestFixture>
    {
        private WorkingFormControllerTestFixture _fixture;

        public WorkingFormControllerTest(WorkingFormControllerTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task Test01_GetListOfAllWorkingFormsWithValidGuid()
        {
            // Setup
            Guid designShopId = _fixture._designShop.Id;
            List<DesignShopWorkingForm> listToCheck = await _fixture._applicationTestDbContext.DesignShopWorkingForm
                .Where(dswf => dswf.DesignShopId == designShopId)
                .OrderBy(dswf => dswf.Order)
                .Include(dswf => dswf.WorkingForm)
                .ToListAsync();

            // Act
            var result = await _fixture._controller.GetListOfWorkingForms(designShopId);

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<List<WorkingFormViewModel>>>(result);
            // Then, get the list while checking it's a list of WorkingFormViewModels
            List<WorkingFormViewModel> theList = Assert.IsType<List<WorkingFormViewModel>>(firstResult.Value);
            // Assert that the list counts 5 items
            Assert.True(theList.Count.Equals(listToCheck.Count));
            WorkingFormViewModel wfmv;
            DesignShopWorkingForm dswf;
            bool currentWorkingFormFound = false;
            for (int i = 0; i < theList.Count; i++)
            {
                wfmv = theList[i];
                dswf = listToCheck[i];
                Assert.Equal(dswf.Id, wfmv.Id);
                Assert.Equal(dswf.WorkingForm.Name, wfmv.Name);
                Assert.Equal(i + 1, dswf.Order);
                if (_fixture._currentWorkingForm.Id == wfmv.Id)
                {
                    currentWorkingFormFound = true;
                    //Assert.Equal(_fixture._designShop.CurrentDesignShopWorkingForm.Id, wfmv.Id);
                    //Assert.Equal(_fixture._designShop.CurrentDesignShopWorkingForm.WorkingForm.Name, wfmv.Name);
                    //Assert.Equal(_fixture._designShop.CurrentDesignShopWorkingForm.Id, dswf.Id);
                }
            }
            Assert.True(currentWorkingFormFound);
        }

        [Fact]
        public async Task Test02_CheckListWithExpectedList()
        {
            // Act
            var result = await _fixture._controller.GetListOfWorkingForms(_fixture._designShop.Id);

        }

        [Fact]
        public async Task Test03_GetListOfAllWorkingFormsWithInvalidGuid()
        {
            // Act
            var result = await _fixture._controller.GetListOfWorkingForms(new Guid());

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<List<WorkingFormViewModel>>>(result);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(firstResult.Result);
        }

        [Fact]
        public async Task Test04_GetCurrentWorkingFormOfValidDesignShop()
        {
            // Act
            var result = await _fixture._controller.GetCurrentWorkingFormOfDesignShop(_fixture._designShop.Id);

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<WorkingFormViewModel>>(result);
            // Then, get the list while checking it's a list of WorkingFormViewModels
            WorkingFormViewModel workingForm = Assert.IsType<WorkingFormViewModel>(firstResult.Value);
            // Assert that the id of the fourth item is equal to the current WorkingForm
            Assert.Equal(_fixture._currentWorkingForm.Id, workingForm.Id);
        }

        [Fact]
        public async Task Test05_GetCurrentWorkingFormOfInvalidDesignShop()
        {
            // Act
            var result = await _fixture._controller.GetCurrentWorkingFormOfDesignShop(new Guid());

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<WorkingFormViewModel>>(result);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(firstResult.Result);
        }

        [Fact]
        public async Task Test06_GetCurrentWorkingFormOfInvalidDesignShop()
        {
            // Act
            var result = await _fixture._controller.GetCurrentWorkingFormOfDesignShop(_fixture._designShopWithoutWorkingForms.Id);

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<WorkingFormViewModel>>(result);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(firstResult.Result);
        }

        [Fact]
        public async Task Test07_GetListOfImagesOfValidCurrentWorkingForm()
        {
            // Act
            var result = await _fixture._controller.GetListOfImagesOfWorkingForm(_fixture._currentWorkingForm.Id);

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<List<DownloadImageViewModel>>>(result);
            // Then, get the list while checking it's a list of WorkingFormViewModels
            List<DownloadImageViewModel> imageList = Assert.IsType<List<DownloadImageViewModel>>(firstResult.Value);
            // Assert that the id of the fourth item is equal to the current WorkingForm
            Assert.True(imageList.Count == 15);
        }

        [Fact]
        public async Task Test08_GetListOfImagesOfInvalidCurrentWorkingForm()
        {
            // Act
            var result = await _fixture._controller.GetListOfImagesOfWorkingForm(new Guid());

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<List<DownloadImageViewModel>>>(result);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(firstResult.Result);
        }

        [Fact]
        public async Task Test09_TestNextWorkingFormAsCurrentWorkingForm()
        {
            // First, check if we get the correct workingform
            // Act
            var result = await _fixture._controller.SetNextWorkingFormOfDesignShopAPI(_fixture._designShop.Id);

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<WorkingFormViewModel>>(result);
            // Then, get the list while checking it's a list of WorkingFormViewModels
            WorkingFormViewModel workingForm = Assert.IsType<WorkingFormViewModel>(firstResult.Value);
            // Assert that the id of the fourth item is equal to the current WorkingForm
            Assert.Equal(_fixture._nextWorkingForm.Id, workingForm.Id);

            // The next call should generate a 404 error because the previous call got the last workingform
            // Act
            var nextResult = await _fixture._controller.SetNextWorkingFormOfDesignShopAPI(_fixture._designShop.Id);

            // Assert
            // First assert: did we get an actionresult with a list
            var SecondResult = Assert.IsType<ActionResult<WorkingFormViewModel>>(nextResult);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(SecondResult.Result);

        }

        [Fact]
        public async Task Test10_GetNextWorkingFormOfInvalidDesignShop()
        {
            // Act
            var result = await _fixture._controller.SetNextWorkingFormOfDesignShopAPI(_fixture._designShopWithoutWorkingForms.Id);

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<WorkingFormViewModel>>(result);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(firstResult.Result);
        }

        [Fact]
        public async Task Test11_GetNextWorkingFormOfNonExistingDesignShop()
        {
            // Act
            var result = await _fixture._controller.SetNextWorkingFormOfDesignShopAPI(new Guid());

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<WorkingFormViewModel>>(result);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(firstResult.Result);
        }

        [Fact]
        public async Task Test12_ChangeOrderOfWorkingForms()
        {
            string workListAsString;
            int counter;
            int numberOfItems;
            List<DesignShopWorkingForm> workList = await helperGetWorkingForms();

            // first stage: reorder backwards
            workListAsString = string.Join(",", getIdsAsStringArray(workList.OrderByDescending(dswf=>dswf.Order).ToList()));
            _fixture._controller.ChangeOrderOfWorkingForms(workListAsString);
            List<DesignShopWorkingForm> reorderdList = await helperGetWorkingForms();
            Assert.Equal(workList.Count(), reorderdList.Count());
            
            counter = 0;
            numberOfItems = workList.Count();
            for (counter = 0; counter < numberOfItems; counter++)
            {
                Assert.True(compareValues(workList[numberOfItems - counter - 1], reorderdList[counter], counter + 1));
            }

        }

        private async Task<List<DesignShopWorkingForm>> helperGetWorkingForms()
        {
            return await _fixture._applicationTestDbContext.DesignShopWorkingForm
                .Where(dswf => dswf.DesignShopId == _fixture._designShop.Id)
                .OrderBy(dswf => dswf.Order)
                .ToListAsync();
        }

        private string[] getIdsAsStringArray(List<DesignShopWorkingForm> workList)
        {
            return workList.Select(dswf => dswf.Id.ToString()).ToArray();
        }

        private bool compareValues(DesignShopWorkingForm original, DesignShopWorkingForm reorderd, int position)
        {
            return original.Id == reorderd.Id
                && reorderd.Order == position;
        }
    }
}