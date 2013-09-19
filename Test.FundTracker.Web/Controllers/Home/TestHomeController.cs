using FuncTracker.Web.Controllers;
using NUnit.Framework;

namespace Test.FundTracker.Web.Controllers.Home
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
    }
}
