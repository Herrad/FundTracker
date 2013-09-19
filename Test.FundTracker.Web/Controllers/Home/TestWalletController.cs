using System.Web.Mvc;
using FuncTracker.Web.Controllers;
using FuncTracker.Web.Controllers.ValidationFailure;
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
        public void CreateWallet_redirects_to_SuccessfullyCreated()
        {
            var walletController = new WalletController();
            var result = walletController.CreateWallet("foo");

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = ((RedirectToRouteResult)result);
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("SuccessfullyCreated"));
        }

        [TestCase(null)]
        [TestCase("")]
        public void CreateWallet_redirects_to_HomeController_Index_action_with_validation_message_if_name_is_null_or_empty(string name)
        {
            var walletController = new WalletController();
            var result = walletController.CreateWallet(null);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = ((RedirectToRouteResult)result);
            Assert.That(redirectResult.RouteValues["controller"], Is.EqualTo("Home"));
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Index"));
            Assert.That(redirectResult.RouteValues["failure"], Is.TypeOf<NoNameValidationFailure>());
        }
    }
}