using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Test.FundTracker.Domain;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestWalletViewModelFormatter
    {
        [Test]
        public void Sets_WalletName_and_Amount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder();

            var wallet = new Wallet(new FakeEventReciever(), new WalletIdentification(walletName), 0, null);
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

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new FakeEventReciever(), walletIdentification, 0, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange(walletIdentification, 123m));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet);
            
            Assert.That(result.WithdrawalTilesViewModel, Is.Not.Null);
        }
    }
}