using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestWithdrawalController
    {
        [Test]
        public void Create_returns_view_with_empty_ViewName_set()
        {
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalController = new WithdrawalController(walletService);

            var result = withdrawalController.Create(null);

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Create_sets_WalletName_on_ViewModel()
        {
            const string walletName = "foo name";
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalController = new WithdrawalController(walletService);
            var result = withdrawalController.Create(walletName);

            var model = (CreateWithdrawalViewModel) result.Model;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.WalletName, Is.EqualTo(walletName));
        }

        [Test]
        public void AddNew_redirects_to_Wallet_display_page()
        {
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(Arg<WalletIdentification>.Is.Anything))
                .Return(withdrawalExposer);

            var withdrawalController = new WithdrawalController(walletService);
            var result = withdrawalController.AddNew("foo name", 123m);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Display"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Wallet"));
        }

        [Test]
        public void AddNew_sets_wallet_name_in_RouteValues()
        {
            const string walletName = "foo name";

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(Arg<WalletIdentification>.Is.Anything))
                .Return(withdrawalExposer);

            var withdrawalController = new WithdrawalController(walletService);

            var result = withdrawalController.AddNew(walletName, 123m);

            Assert.That(result.RouteValues["walletName"], Is.Not.Null);
            Assert.That(result.RouteValues["walletName"], Is.EqualTo(walletName));
        }

        [Test]
        public void AddNew_puts_new_withdrawal_on_wallet_with_identification_set_and_amount_inverted()
        {
            var expectedWalletIdentification = new WalletIdentification("foo");
            const int amountToWithdraw = 123;
            const decimal expectedAmountGivenToWallet = 0-amountToWithdraw;

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(expectedWalletIdentification))
                .Return(withdrawalExposer);

            var withdrawalController = new WithdrawalController(walletService);

            withdrawalController.AddNew("foo", amountToWithdraw);

            Assert.That(withdrawalExposer.WithdrawalAdded, Is.Not.Null);
            Assert.That(withdrawalExposer.WithdrawalAdded.Identification, Is.EqualTo(expectedWalletIdentification));
            Assert.That(withdrawalExposer.WithdrawalAdded.Amount, Is.EqualTo(expectedAmountGivenToWallet));
        }
    }
}