using System.Web.Mvc;
using FundTracker.Web.Controllers;
using FundTracker.Web.ViewModels;
using NUnit.Framework;

namespace Test.FundTracker.Web.Controllers
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
        public void CreateWallet_redirects_to_HomeController_ValidationFailure_action_if_name_is_null_or_empty(string name)
        {
            var walletController = new WalletController();
            var result = walletController.CreateWallet(name);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = ((RedirectToRouteResult)result);
            Assert.That(redirectResult.RouteValues["controller"], Is.EqualTo("Home"));
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("ValidationFailure"));
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
            Assert.That(failureValue, Is.TypeOf<string>());

            Assert.That(failureValue, Is.EqualTo("You need to put in a name for this wallet"));
        }

        [Test]
        public void Display_returns_a_view_with_no_ViewName_set()
        {
            var walletController = new WalletController();
            var viewResult = walletController.Display(null);

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Display_builds_WalletViewModel_with_name_set()
        {
            var walletController = new WalletController();
            var viewResult = walletController.Display("foo wallet");

            var viewModel = (WalletViewModel) viewResult.Model;

            Assert.That(viewModel.Name, Is.EqualTo("foo wallet"));
        }
    }
}