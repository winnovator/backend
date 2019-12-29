using Microsoft.Extensions.Logging;
using Moq;
using WInnovator.API;
using WInnovator.Models;
using WInnovatorTest.Data;

namespace WInnovatorTest.API.Fixtures
{
    public class UploadImageControllerTestFixture : DbContextTest
    {
        public ILogger<UploadImageController> _logger;
        public UploadImageController _controller;
        public DesignShop _designShopWithCurrentWorkingForm;
        public DesignShop _designShopWithoutCurrentWorkingForm;
        public DesignShopWorkingForm _currentWorkingForm;
        public ImageStore _imageStore;

        public UploadImageControllerTestFixture()
        {
            // Arrange
            _logger = Mock.Of<ILogger<UploadImageController>>();
            _controller = new UploadImageController(_applicationTestDbContext, _logger);

            MockData();
        }

        private void MockData()
        {
            MockDesignShops();
            MockWorkingForms();
        }

        private void MockDesignShops()
        {
            // Setup several empty designshops
            for (var i = 0; i < 6; i++)
            {
                _applicationTestDbContext.DesignShop.Add(new DesignShop());
            }

            // Add one we'll remember
            _designShopWithCurrentWorkingForm = new DesignShop();
            _applicationTestDbContext.DesignShop.Add(_designShopWithCurrentWorkingForm);

            // Add another one, but we'll not going to add workingforms to this one
            _designShopWithoutCurrentWorkingForm = new DesignShop();
            _applicationTestDbContext.DesignShop.Add(_designShopWithoutCurrentWorkingForm);

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
                workingForm = new WorkingForm() {Name = $"Workingform {i}"};
                _applicationTestDbContext.WorkingForm.Add(workingForm);
                designShopWorkingForm = new DesignShopWorkingForm()
                    {DesignShop = _designShopWithCurrentWorkingForm, WorkingForm = workingForm, Order = i};
                _applicationTestDbContext.DesignShopWorkingForm.Add(designShopWorkingForm);
                if (i == 4) { 
                    _currentWorkingForm = designShopWorkingForm;
                    _currentWorkingForm.UploadEnabled = true;
                    _currentWorkingForm.IsCurrentWorkingForm = true;
                }
            }

            _applicationTestDbContext.SaveChanges();
        }
    }
}