using System.Web.Mvc;
using FundTracker.Web.Controllers;
using FundTracker.Web.Controllers.ActionHelpers;
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
            var walletController = new WalletController(new CreateWalletValidation());
            var viewResult = walletController.SuccessfullyCreated(null);

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SuccessfullyCreated_sets_WalletName_on_ViewModel()
        {
            const string walletName = "foo walletName";

            var walletController = new WalletController(new CreateWalletValidation());
            
            var viewResult = walletController.SuccessfullyCreated(walletName);

            var viewModel = (SuccessfullyCreatedWalletViewModel) viewResult.Model;
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Name, Is.EqualTo(walletName));
        }

        [Test]
        public void CreateWallet_redirects_to_SuccessfullyCreated_passing_name()
        {
            const string walletName = "foo";

            var walletController = new WalletController(new CreateWalletValidation());
            
            var result = walletController.CreateWallet(walletName);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectResult = (RedirectToRouteResult)result;
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("SuccessfullyCreated"));

            var walletNameValue = redirectResult.RouteValues["walletName"];
            Assert.That(walletNameValue, Is.EqualTo(walletName));
        }

        [Test]
        public void AddFunds_redirects_to_DisplayWallet_passing_name_and_funds()
        {
            var walletController = new WalletController(new CreateWalletValidation());

            const string expectedName = "fooName";
            const decimal expectedFunds = 100.00m;

            var result = walletController.AddFunds(expectedName, expectedFunds);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectResult = (RedirectToRouteResult) result;

            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Display"));
            Assert.That(redirectResult.RouteValues["walletName"], Is.EqualTo(expectedName));
            Assert.That(redirectResult.RouteValues["availableFunds"], Is.EqualTo(expectedFunds));
        }
        
        [TestCase(null)]
        [TestCase("")]
        public void CreateWallet_redirects_to_HomeController_ValidationFailure_action_if_name_is_null_or_empty(string name)
        {
            var walletController = new WalletController(new CreateWalletValidation());
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
            var walletController = new WalletController(new CreateWalletValidation());
            var result = walletController.CreateWallet(name);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());
            var redirectResult = ((RedirectToRouteResult)result);

            var failureValue = redirectResult.RouteValues["failure"];
            Assert.That(failureValue, Is.TypeOf<string>());

            Assert.That(failureValue, Is.EqualTo("You need to put in a name for this wallet"));
        }

        [Test]
        public void Display_returns_a_view_with_ViewName_set_to_Display()
        {
            var walletController = new WalletController(new CreateWalletValidation());
            var viewResult = walletController.Display(null, 0);

            Assert.That(viewResult.ViewName, Is.EqualTo("Display"));
        }

        [Test]
        public void Display_builds_WalletViewModel_with_name_and_funds_set()
        {
            var walletController = new WalletController(new CreateWalletValidation());
            var viewResult = walletController.Display("foo wallet", 123m);

            var viewModel = (WalletViewModel) viewResult.Model;

            Assert.That(viewModel.Name, Is.EqualTo("foo wallet"));
            Assert.That(viewModel.AvailableFunds, Is.EqualTo(123m));
        }

        [Test]
        public void Display_with_no_funds_sets_funds_to_0()
        {
            var walletController = new WalletController(new CreateWalletValidation());
            var viewResult = walletController.DisplayNoFunds("foo wallet");

            var viewModel = (WalletViewModel)viewResult.Model;
            Assert.That(viewModel.AvailableFunds, Is.EqualTo(0));
        }
    }
}