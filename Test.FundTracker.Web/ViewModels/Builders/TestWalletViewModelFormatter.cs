using FundTracker.Domain;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;

namespace Test.FundTracker.Web.ViewModels.Builders
{
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