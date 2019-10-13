using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
        public async Task Test1_GetListOfAllWorkingFormsWithValidGuid()
        {
            // Act
            var result = await _fixture._controller.GetListOfWorkingForms(_fixture._designShop.Id);

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<List<WorkingFormViewModel>>>(result);
            // Then, get the list while checking it's a list of WorkingFormViewModels
            List<WorkingFormViewModel> theList = Assert.IsType<List<WorkingFormViewModel>>(firstResult.Value);
            // Assert that the list counts 5 items
            Assert.True(theList.Count == 5);
            // Assert that the id of the fourth item is equal to the current WorkingForm
            Assert.Equal(_fixture._currentWorkingForm.Id, theList[3].Id);
        }

        [Fact]
        public async Task Test2_GetListOfAllWorkingFormsWithInvalidGuid()
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
        public async Task Test3_GetCurrentWorkingFormOfValidDesignShop()
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
        public async Task Test4_GetCurrentWorkingFormOfInvalidDesignShop()
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
        public async Task Test5_GetListOfImagesOfValidCurrentWorkingForm()
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
        public async Task Test6_GetListOfImagesOfInvalidCurrentWorkingForm()
        {
            // Act
            var result = await _fixture._controller.GetListOfImagesOfWorkingForm(new Guid());

            // Assert
            // First assert: did we get an actionresult with a list
            var firstResult = Assert.IsType<ActionResult<List<DownloadImageViewModel>>>(result);
            // Then, check if the result is a NotFound
            Assert.IsType<NotFoundResult>(firstResult.Result);
        }
    }
}