using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Domain.Events;
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

            var wallet = new Wallet(new FakeEventReciever(), new WalletIdentification(null), 0, null);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Wallets_with_the_same_Identification_are_equal()
        {
            var identification = new WalletIdentification("foo name");

            var wallet1 = new Wallet(new FakeEventReciever(), identification, 0, null);
            var wallet2 = new Wallet(new FakeEventReciever(), identification, 0, null);

            Assert.That(wallet1.Equals(wallet2), "wallets are not the same");
        }

        [Test]
        public void Publishes_event_when_funds_change()
        {
            const decimal expectedFunds = 150m;

            var eventBus = new FakeEventReciever();
            var wallet = new Wallet(eventBus, new WalletIdentification(null), 0, null);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            var eventPublished = eventBus.EventPublished;
            Assert.That(eventPublished, Is.Not.Null);
            Assert.That(eventPublished, Is.TypeOf<WalletFundsChanged>());

            var walletFundsChanged = (WalletFundsChanged) eventPublished;

            Assert.That(walletFundsChanged.Wallet, Is.EqualTo(wallet));
            Assert.That(walletFundsChanged.Wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Stores_RecurringChange_in_list()
        {
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(new FakeEventReciever(), walletIdentification, 0, recurringChanges);

            var recurringChange = new RecurringChange(walletIdentification, 123);
            wallet.CreateChange(recurringChange);

            Assert.That(recurringChanges.Contains(recurringChange));
        }

        [Test]
        public void Applies_RecurringChange_to_Funds()
        {
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(new FakeEventReciever(), walletIdentification, 100, recurringChanges);

            var recurringChange = new RecurringChange(walletIdentification, -25);
            wallet.CreateChange(recurringChange);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(75));
        }

        [Test]
        public void CreatingWithdrawal_sends_event_to_bus()
        {
            const int expectedAmount = 123;

            var eventBus = new FakeEventReciever();
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(eventBus, walletIdentification, 0, recurringChanges);

            var recurringChange = new RecurringChange(walletIdentification, expectedAmount);
            wallet.CreateChange(recurringChange);

            Assert.That(eventBus.EventPublished, Is.TypeOf<RecurringChangeCreated>());

            var recurringChangeCreated = (RecurringChangeCreated) eventBus.EventPublished;

            Assert.That(recurringChangeCreated.Change, Is.Not.Null);
            Assert.That(recurringChangeCreated.Change.Amount, Is.EqualTo(expectedAmount));
            Assert.That(recurringChangeCreated.Change.Identification, Is.EqualTo(walletIdentification));
        }
    }
}
