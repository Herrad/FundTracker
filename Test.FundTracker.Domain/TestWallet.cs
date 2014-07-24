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

            var wallet1 = new Wallet(new LastEventPublishedReporter(), identification, null);
            var wallet2 = new Wallet(new LastEventPublishedReporter(), identification, null);

            Assert.That(wallet1.Equals(wallet2), "wallets are not the same");
        }

        [Test]
        public void Stores_RecurringChange_in_list()
        {
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, recurringChanges);

            var recurringChange = new RecurringChange("foo", 123, null);
            wallet.CreateChange(recurringChange);

            Assert.That(recurringChanges.Contains(recurringChange));
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
            var wallet = new Wallet(eventBus, walletIdentification, recurringChanges);

            var recurringChange = new RecurringChange(expectedChangeName, expectedAmount, new OneShotRule(expectedStartDate, null));
            wallet.CreateChange(recurringChange);

            Assert.That(eventBus.LastEventPublished, Is.TypeOf<RecurringChangeCreated>());

            var recurringChangeCreated = (RecurringChangeCreated) eventBus.LastEventPublished;

            Assert.That(recurringChangeCreated.Change, Is.Not.Null);
            Assert.That(recurringChangeCreated.Change.Amount, Is.EqualTo(expectedAmount));
            Assert.That(recurringChangeCreated.Change.Name, Is.EqualTo(expectedChangeName));
            Assert.That(recurringChangeCreated.Change.StartDate, Is.EqualTo(expectedStartDate));
        }
    }
}
