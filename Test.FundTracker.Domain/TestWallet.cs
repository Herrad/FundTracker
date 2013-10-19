using FundTracker.Domain;
using NUnit.Framework;

namespace Test.FundTracker.Domain
{
    [TestFixture]
    public class TestWallet
    {
        [Test]
        public void Adding_funds_to_a_wallet_increments_AvailableFunds()
        {
            const decimal expectedFunds = 150m;
            
            var wallet = new Wallet(null);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Wallets_with_the_same_name_are_equal()
        {
            const string walletName = "foo name";

            var wallet1 = new Wallet(walletName);
            var wallet2 = new Wallet(walletName);

            Assert.That(wallet1.Equals(wallet2), "wallets are not the same");
        }
    }
}
