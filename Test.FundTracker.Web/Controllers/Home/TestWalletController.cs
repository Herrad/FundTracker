using System.Web.Mvc;
using FuncTracker.Web.Controllers;
using NUnit.Framework;

namespace Test.FundTracker.Web.Controllers.Home
{
    [TestFixture]
    public class TestWalletController
    {
        [Test]
        public void SuccessfullyCreated_returns_ViewResult_with_empty_ViewName()
        {
            var walletController = new WalletController();
            var viewResult = walletController.SuccessfullyCreated();

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void CreateWallet_returns_ViewResult_with_empty_ViewName()
        {
            var walletController = new WalletController();
            var viewResult = walletController.CreateWallet("foo");

            Assert.That(viewResult, Is.TypeOf<ViewResult>());
            Assert.That(((ViewResult)viewResult).ViewName, Is.EqualTo(string.Empty));
        }

        [TestCase(null)]
        [TestCase("")]
        public void CreateWallet_redirects_to_HomeController_Index_action_if_name_is_null_or_empty(string name)
        {
            var walletController = new WalletController();
            var result = walletController.CreateWallet(null);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = ((RedirectToRouteResult)result);
            Assert.That(redirectResult.RouteValues["controller"], Is.EqualTo("Home"));
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Index"));
        }
    }
}