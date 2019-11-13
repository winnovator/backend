using Microsoft.Extensions.Logging;
using Moq;
using WInnovator.API;
using WInnovator.Models;
using WInnovatorTest.Data;

namespace WInnovatorTest.API.Fixtures
{
    public class WorkingFormControllerTestFixture : DbContextTest
    {
        public ILogger<WorkingFormController> _logger;
        public WorkingFormController _controller;
        public DesignShop _designShop;
        public DesignShop _designShopWithoutWorkingForms;
        public DesignShopWorkingForm _currentWorkingForm;

        public WorkingFormControllerTestFixture()
        {
            // Arrange
            _logger = Mock.Of<ILogger<WorkingFormController>>();
            _controller = new WorkingFormController(_applicationTestDbContext, _logger);

            MockData();
        }

        private void MockData()
        {
            MockDesignShops();
            MockWorkingForms();
            MockImages();
        }

        private void MockDesignShops()
        {
            // Setup several empty designshops
            for (var i = 0; i < 6; i++)
            {
                _applicationTestDbContext.DesignShop.Add(new DesignShop());
            }

            // Add one we'll remember
            _designShop = new DesignShop();
            _applicationTestDbContext.DesignShop.Add(_designShop);

            // And one without any workingforms
            _designShopWithoutWorkingForms = new DesignShop();
            _applicationTestDbContext.DesignShop.Add(_designShopWithoutWorkingForms);

            // And throw in another empty designshops
            for (var i = 0; i < 6; i++)
            {
                _applicationTestDbContext.DesignShop.Add(new DesignShop());
            }

            _applicationTestDbContext.SaveChanges();
        }

        private void MockWorkingForms()
        {
            WorkingForm workingForm;
            DesignShopWorkingForm designShopWorkingForm;
            // We'll add 5 workingforms to the saved designshop
            // The fourth workingform will act as the current workingform
            for (var i = 1; i < 6; i++)
            {
                workingForm = new WorkingForm() {Description = $"Workingform {i}"};
                _applicationTestDbContext.WorkingForm.Add(workingForm);
                designShopWorkingForm = new DesignShopWorkingForm()
                    {DesignShop = _designShop, WorkingForm = workingForm, Order = i};
                if (i == 4)
                {
                    _currentWorkingForm = designShopWorkingForm;
                    _currentWorkingForm.IsCurrentWorkingForm = true;
                }
                _applicationTestDbContext.DesignShopWorkingForm.Add(designShopWorkingForm);
            }

            //_designShop.CurrentDesignShopWorkingForm = _currentWorkingForm;

            _applicationTestDbContext.SaveChanges();
        }

        private void MockImages()
        {
            ImageStore imageStore;
            for (var i = 1; i < 16; i++)
            {
                imageStore = new ImageStore() {Mimetype = $"image/{i}", DesignShopWorkingForm = _currentWorkingForm};
                _applicationTestDbContext.ImageStore.Add(imageStore);
            }

            _applicationTestDbContext.SaveChanges();
        }
    }
}