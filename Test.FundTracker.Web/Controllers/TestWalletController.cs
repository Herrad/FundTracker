using System.Web.Mvc;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.ViewModels;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestWalletController
    {
        [Test]
        public void SuccessfullyCreated_returns_ViewResult_with_empty_ViewName()
        {
            var walletController = new WalletController(new CreateWalletValidation(new WalletNameValidator(), null), null, new WalletViewModelBuilder());
            var viewResult = walletController.SuccessfullyCreated(null);

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        }

        [Test]
        public void SuccessfullyCreated_sets_WalletName_on_ViewModel()
        {
            const string walletName = "foo walletName";

            var walletController = new WalletController(new CreateWalletValidation(new WalletNameValidator(), null), null, new WalletViewModelBuilder());
            
            var viewResult = walletController.SuccessfullyCreated(walletName);

            var viewModel = (SuccessfullyCreatedWalletViewModel) viewResult.Model;
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Name, Is.EqualTo(walletName));
        }

        [Test]
        public void CreateWallet_redirects_to_SuccessfullyCreated_passing_name()
        {
            const string walletName = "foo";

            var walletController = new WalletController(new CreateWalletValidation(new WalletNameValidator(), MockRepository.GenerateStub<ICreateWallets>()), null, new WalletViewModelBuilder());
            
            var result = walletController.CreateWallet(walletName);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectResult = (RedirectToRouteResult)result;
            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("SuccessfullyCreated"));

            var walletNameValue = redirectResult.RouteValues["walletName"];
            Assert.That(walletNameValue, Is.EqualTo(walletName));
        }

        [Test]
        public void AddFunds_redirects_to_DisplayWallet_passing_name()
        {
            const string expectedName = "fooName";
            const decimal fundsToAdd = 100.00m;

            var walletProvider = MockRepository.GenerateStub<IProvideWallets>();
            walletProvider
                .Stub(x => x.GetBy(expectedName))
                .Return(MockRepository.GenerateStub<IWallet>());
            
            var walletController = new WalletController(new CreateWalletValidation(new WalletNameValidator(), null), walletProvider, new WalletViewModelBuilder());


            var result = walletController.AddFunds(expectedName, fundsToAdd);

            Assert.That(result, Is.TypeOf<RedirectToRouteResult>());

            var redirectResult = (RedirectToRouteResult) result;

            Assert.That(redirectResult.RouteValues["action"], Is.EqualTo("Display"));
            Assert.That(redirectResult.RouteValues["walletName"], Is.EqualTo(expectedName));
        }

        [Test]
        public void AddFunds_adds_funds_to_wallet()
        {
            const string name = "foo name";
            const decimal fundsToAdd = 123m;

            var wallet = MockRepository.GenerateMock<IWallet>();

            var walletProvider = MockRepository.GenerateStub<IProvideWallets>();
            walletProvider
                .Stub(x => x.GetBy(name))
                .Return(wallet);

            var controller = new WalletController(null, walletProvider, null);

            controller.AddFunds(name, fundsToAdd);

            wallet.AssertWasCalled(
                x => x.AddFunds(fundsToAdd), 
                c => c.Repeat.Once());
        }
        
        [TestCase(null)]
        [TestCase("")]
        public void CreateWallet_redirects_to_HomeController_ValidationFailure_action_if_name_is_null_or_empty(string name)
        {
            var walletController = new WalletController(new CreateWalletValidation(new WalletNameValidator(), MockRepository.GenerateStub<ICreateWallets>()), null, new WalletViewModelBuilder());
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
            var walletController = new WalletController(new CreateWalletValidation(new WalletNameValidator(), MockRepository.GenerateStub<ICreateWallets>()), null, new WalletViewModelBuilder());
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
            var walletProvider = MockRepository.GenerateStub<IProvideWallets>();
            var formatWalletsAsViewModels = MockRepository.GenerateStub<IFormatWalletsAsViewModels>();

            var walletController = new WalletController(null, walletProvider, formatWalletsAsViewModels);
            var viewResult = walletController.Display(null);

            Assert.That(viewResult.ViewName, Is.EqualTo("Display"));
        }

        [Test]
        public void Display_gives_wallet_to_ViewModelBuilder()
        {
            const string walletName = "foo wallet";
            var wallet = new Wallet(walletName);

            var walletProvider = MockRepository.GenerateStub<IProvideWallets>();
            walletProvider
                .Stub(x => x.GetBy(walletName))
                .Return(wallet);
            
            var walletViewModelBuilder = MockRepository.GenerateMock<IFormatWalletsAsViewModels>();

            var walletController = new WalletController(null, walletProvider, walletViewModelBuilder);
            walletController.Display(walletName);

            walletViewModelBuilder
                .AssertWasCalled(
                x => x.FormatWalletAsViewModel(wallet),
                c => c.Repeat.Once());
        }
    }

    [TestFixture]
    public class TestWalletViewModelFormatter
    {
        [Test]
        public void Sets_WalletName_and_Amount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder();

            var wallet = new Wallet(walletName);
            wallet.AddFunds(123m);
            var result = viewModelFormatter.FormatWalletAsViewModel(wallet);

            Assert.That(result.Name, Is.EqualTo(walletName));
            Assert.That(result.AvailableFunds, Is.EqualTo(123m));
        }

        [Test]
        public void Sets_WithdrawalTile_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder();

            var wallet = new Wallet(walletName);
            wallet.AddFunds(123m);

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet);
            
            Assert.That(result.WithdrawalTilesViewModel, Is.Not.Null);
        }
    }
}