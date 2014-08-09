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

            const int amount = 123;
            const string changeName = "foo";
            wallet.CreateChange(changeName, amount, new OneShotRule(new DateTime(1, 2, 3), null));

            Assert.That(recurringChanges[0].Name, Is.EqualTo(changeName));
            Assert.That(recurringChanges[0].Amount, Is.EqualTo(amount));
        }

        [Test]
        public void CreatingWithdrawal_sends_event_to_bus()
        {
            const string expectedChangeName = "foo";
            const int expectedAmount = 123;
            var startDate = new DateTime(1, 2, 3);
            var expectedStartDate = startDate.ToString("yyyy-MM-dd");

            var eventBus = new LastEventPublishedReporter();
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(eventBus, walletIdentification, recurringChanges);

            wallet.CreateChange(expectedChangeName, expectedAmount, new OneShotRule(startDate, null));

            Assert.That(eventBus.LastEventPublished, Is.TypeOf<RecurringChangeCreated>());

            var recurringChangeCreated = (RecurringChangeCreated) eventBus.LastEventPublished;

            Assert.That(recurringChangeCreated.RecurringChangeValues, Is.Not.Null);
            Assert.That(recurringChangeCreated.RecurringChangeValues.Amount, Is.EqualTo(expectedAmount));
            Assert.That(recurringChangeCreated.RecurringChangeValues.Name, Is.EqualTo(expectedChangeName));
            Assert.That(recurringChangeCreated.RecurringChangeValues.StartDate, Is.EqualTo(expectedStartDate));
        }

        [Test]
        public void GetNextId_returns_1_if_no_RecurringChanges_present()
        {
            var eventBus = new LastEventPublishedReporter();
            var recurringChanges = new List<RecurringChange>();
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(eventBus, walletIdentification, recurringChanges);

            wallet.CreateChange("foo", 123, new OneShotRule(DateTime.Today, null));

            Assert.That(recurringChanges[0].Id, Is.EqualTo(1));
        }

        [Test]
        public void GetNextId_returns_next_highest_id_if_no_RecurringChanges_present()
        {
            var eventBus = new LastEventPublishedReporter();
            var recurringChanges = new List<RecurringChange>{new RecurringChange(4, null, 0, null)};
            var walletIdentification = new WalletIdentification(null);
            var wallet = new Wallet(eventBus, walletIdentification, recurringChanges);

            wallet.CreateChange("foo", 123, new OneShotRule(DateTime.Today, null));

            Assert.That(recurringChanges[1].Id, Is.EqualTo(5));
        }
    }
}
