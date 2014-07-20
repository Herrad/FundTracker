using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Domain.Events;
using FundTracker.Domain.RecurranceRules;
using NUnit.Framework;

namespace Test.FundTracker.Domain
{
    [TestFixture]
    public class TestWallet
    {

        [Test]
        public void Wallets_with_the_same_Identification_are_equal()
        {
            var identification = new WalletIdentification("foo name");

            var wallet1 = new Wallet(new LastEventPublishedReporter(), identification, 0, null);
            var wallet2 = new Wallet(new LastEventPublishedReporter(), identification, 0, null);

            Assert.That(wallet1.Equals(wallet2), "wallets are not the same");
        }

        [Test]
        public void Stores_RecurringChange_in_list()
        {
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, 0, recurringChanges);

            var recurringChange = new RecurringChange("foo", 123, new DateTime(1, 2, 3), null);
            wallet.CreateChange(recurringChange);

            Assert.That(recurringChanges.Contains(recurringChange));
        }

        [Test]
        public void Applies_RecurringChange_to_Funds()
        {
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, 100, recurringChanges);

            var recurringChange = new RecurringChange("foo", -25, new DateTime(1, 2, 3), null);
            wallet.CreateChange(recurringChange);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(75));
        }

        [Test]
        public void CreatingWithdrawal_sends_event_to_bus()
        {
            const string expectedChangeName = "foo";
            const int expectedAmount = 123;
            var expectedStartDate = new DateTime(1, 2, 3);

            var eventBus = new LastEventPublishedReporter();
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(eventBus, walletIdentification, 0, recurringChanges);

            var recurringChange = new RecurringChange(expectedChangeName, expectedAmount, expectedStartDate, null);
            wallet.CreateChange(recurringChange);

            Assert.That(eventBus.LastEventPublished, Is.TypeOf<RecurringChangeCreated>());

            var recurringChangeCreated = (RecurringChangeCreated) eventBus.LastEventPublished;

            Assert.That(recurringChangeCreated.Change, Is.Not.Null);
            Assert.That(recurringChangeCreated.Change.Amount, Is.EqualTo(expectedAmount));
            Assert.That(recurringChangeCreated.Change.Name, Is.EqualTo(expectedChangeName));
            Assert.That(recurringChangeCreated.Change.StartDate, Is.EqualTo(expectedStartDate));
        }
    }

    [TestFixture]
    public class TestWalletFunds
    {

        [Test]
        public void Publishes_event_when_funds_change()
        {
            const decimal expectedFunds = 150m;

            var eventBus = new LastEventPublishedReporter();
            var wallet = new Wallet(eventBus, new WalletIdentification(null), 0, null);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            var eventPublished = eventBus.LastEventPublished;
            Assert.That(eventPublished, Is.Not.Null);
            Assert.That(eventPublished, Is.TypeOf<WalletFundsChanged>());

            var walletFundsChanged = (WalletFundsChanged)eventPublished;

            Assert.That(walletFundsChanged.Wallet, Is.EqualTo(wallet));
            Assert.That(walletFundsChanged.Wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Adding_funds_to_a_wallet_increments_AvailableFunds()
        {
            const decimal expectedFunds = 150m;

            var wallet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification(null), 0, null);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Getting_AvailableFunds_calculates_all_RecurringChanges_since_January_2013()
        {
            const decimal expectedFunds = 400;

            var startDate = new DateTime(2014, 07, 01);
            var recurringChanges = new List<RecurringChange>()
            {
                new RecurringChange("Payday", 100m, startDate, new WeeklyRule(startDate))
            };
            var wallet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification(null), 0, recurringChanges);

            var availableFundsInFourWeeks = wallet.GetAvailableFundsFor(startDate.AddDays(28));

            Assert.That(availableFundsInFourWeeks, Is.EqualTo(expectedFunds));
        }
    }
}
