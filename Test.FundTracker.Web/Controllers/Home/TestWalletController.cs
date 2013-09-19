using System.Web.Mvc;
using FundTracker.Web.Controllers;
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
        public void CreateWallet_redirects_to_HomeController_Index_action_if_name_is_null_or_empty(string name)
        {
            var walletController = new WalletController();
            var result = walletController.CreateWallet(name);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = ((RedirectToRouteResult)result);
            Assert.That(redirectResult.RouteValues["controller"], Is.EqualTo("Home"));
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Index"));
        }

        [TestCase(null)]
        [TestCase("")]
        public void CreateWallet_sets_validation_message_if_name_is_null_or_empty(string name)
        {
            var walletController = new WalletController();
            var result = walletController.CreateWallet(name);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = ((RedirectToRouteResult)result);

            var failureValue = redirectResult.RouteValues["failure"];
            Assert.That(failureValue, Is.TypeOf<ValidationFailure>());

            var validationFailureMessage = ((ValidationFailure) failureValue).GetFailureMessage();

            Assert.That(validationFailureMessage, Is.EqualTo("You need to put in a name for this wallet"));
        }
    }
}