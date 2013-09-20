using FundTracker.Web.Controllers;
using FundTracker.Web.ViewModels;
using NUnit.Framework;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestHomeController
    {
        [Test]
        public void Index_returns_ViewResult_with_empty_ViewName()
        {
            var homeController = new HomeController();
            var viewResult = homeController.Index();

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Index_has_ViewModel_set()
        {
            var homeController = new HomeController();
            var viewResult = homeController.ValidationFailure(null);

            Assert.That(viewResult.Model, Is.TypeOf<HomePageViewModel>());
        }

        [Test]
        public void Index_sets_message_on_HomePageViewModel_to_passed_in_ValidationMessage()
        {
            const string validationMessage = "foo fail";

            var homeController = new HomeController();
            var viewResult = homeController.ValidationFailure(validationMessage);

            var homePageViewModel = (HomePageViewModel) viewResult.Model;
            Assert.That(homePageViewModel.ValidationFailure, Is.EqualTo(validationMessage));
        }
    }
}
