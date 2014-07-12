using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers;
using FundTracker.Web.ViewModels;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestRecurringChangeController
    {
        [Test]
        public void Display_returns_view_with_empty_name()
        {
            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IProvideWallets>(), MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>());
            var result = recurringChangeController.Display("foo wallet");

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Display_puts_RecurringChangeListViewModel_on_view()
        {
            const string walletName = "foo wallet";

            var wallet = MockRepository.GenerateStub<IWallet>();
            var provideWallets = MockRepository.GenerateStub<IProvideWallets>();
            provideWallets
                .Stub(x => x.FindFirstWalletWith(new WalletIdentification(walletName)))
                .Return(wallet);

            var recurringChangeListViewModel = new RecurringChangeListViewModel(new List<string>());
            var recurringChangeListViewModelBuilder = MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>();
            recurringChangeListViewModelBuilder
                .Stub(x => x.Build(wallet))
                .Return(recurringChangeListViewModel);


            var recurringChangeController = new RecurringChangeController(provideWallets, recurringChangeListViewModelBuilder);
            var viewResult = recurringChangeController.Display(walletName);

            Assert.That(viewResult.Model, Is.EqualTo(recurringChangeListViewModel));
        }

        [Test]
        public void Create_returns_view_with_empty_ViewName_set()
        {
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalController = new RecurringChangeController(walletService, MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>());

            var result = withdrawalController.CreateWithdrawal(null);

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Create_sets_WalletName_on_ViewModel()
        {
            const string walletName = "foo name";
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalController = new RecurringChangeController(walletService, MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>());
            var result = withdrawalController.CreateWithdrawal(walletName);

            var model = (CreateRecurringChangeViewModel) result.Model;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.WalletName, Is.EqualTo(walletName));
        }

        [Test]
        public void AddNewWithdrawal_redirects_to_Wallet_display_page()
        {
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(Arg<WalletIdentification>.Is.Anything))
                .Return(withdrawalExposer);

            var withdrawalController = new RecurringChangeController(walletService, MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>());
            var result = withdrawalController.AddNewWithdrawal("foo name", "foo withdrawal", 123m);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Display"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Wallet"));
        }

        [Test]
        public void AddNewWithdrawal_sets_wallet_name_in_RouteValues()
        {
            const string walletName = "foo name";

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(Arg<WalletIdentification>.Is.Anything))
                .Return(withdrawalExposer);

            var withdrawalController = new RecurringChangeController(walletService, MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>());

            var result = withdrawalController.AddNewWithdrawal(walletName, "foo withdrawal", 123m);

            Assert.That(result.RouteValues["walletName"], Is.Not.Null);
            Assert.That(result.RouteValues["walletName"], Is.EqualTo(walletName));
        }

        [Test]
        public void AddNew_puts_new_withdrawal_on_wallet_with_identification_set_and_amount_inverted()
        {
            const string walletName = "foo wallet";
            var expectedWalletIdentification = new WalletIdentification(walletName);
            const int amountToWithdraw = 123;
            const decimal expectedAmountGivenToWallet = 0-amountToWithdraw;

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(expectedWalletIdentification))
                .Return(withdrawalExposer);

            var withdrawalController = new RecurringChangeController(walletService, MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>());

            const string withdrawalName = "withdrawal for foo";
            withdrawalController.AddNewWithdrawal(walletName, withdrawalName, amountToWithdraw);

            Assert.That(withdrawalExposer.WithdrawalAdded, Is.Not.Null);
            Assert.That(withdrawalExposer.WithdrawalAdded.Amount, Is.EqualTo(expectedAmountGivenToWallet));
            Assert.That(withdrawalExposer.WithdrawalAdded.Name, Is.EqualTo(withdrawalName));
        }
    }
}