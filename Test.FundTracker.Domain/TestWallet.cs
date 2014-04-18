using FundTracker.Domain;
using FundTracker.Domain.Events;
using MicroEvent;
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

            var wallet = new Wallet(new WalletIdentification(null), 0, new FakeEventReciever());

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Wallets_with_the_same_Identification_are_equal()
        {
            var identification = new WalletIdentification("foo name");

            var wallet1 = new Wallet(identification, 0, new FakeEventReciever());
            var wallet2 = new Wallet(identification, 0, new FakeEventReciever());

            Assert.That(wallet1.Equals(wallet2), "wallets are not the same");
        }

        [Test]
        public void Publishes_event_when_funds_change()
        {
            const decimal expectedFunds = 150m;

            var eventBus = new FakeEventReciever();
            var wallet = new Wallet(new WalletIdentification(null), 0, eventBus);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            var eventPublished = eventBus.EventPublished;
            Assert.That(eventPublished, Is.Not.Null);
            Assert.That(eventPublished, Is.TypeOf<WalletFundsChanged>());

            var walletFundsChanged = (WalletFundsChanged) eventPublished;

            Assert.That(walletFundsChanged.Wallet, Is.EqualTo(wallet));
            Assert.That(walletFundsChanged.Wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }
    }
}
