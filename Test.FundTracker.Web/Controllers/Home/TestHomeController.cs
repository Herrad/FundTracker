using FundTracker.Web.Controllers;
using FundTracker.Web.ViewModels;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers.Home
{
    [TestFixture]
    public class TestHomeController
    {
        [Test]
        public void Index_returns_ViewResult_with_empty_ViewName_when_no_failures_are_passed()
        {
            var homeController = new HomeController();
            var viewResult = homeController.Index(null);

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Index_has_ViewModel_set()
        {
            var homeController = new HomeController();
            var viewResult = homeController.Index(null);

            Assert.That(viewResult.Model, Is.TypeOf<HomePageViewModel>());
        }
    }
}
