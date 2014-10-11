using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Domain.Events;
using FundTracker.Domain.RecurranceRules;
using MicroEvent;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Domain
{
    [TestFixture]
    public class TestWalletFunds
    {

        [Test]
        public void Getting_AvailableFunds_calculates_all_RecurringChanges_since_January_2013()
        {
            const decimal expectedFunds = 400;

            var startDate = new DateTime(2014, 07, 01);
            var recurringChanges = new List<RecurringChange>
            {
                new RecurringChange(0, "Payday", 100m, new WeeklyRule(startDate, null))
            };
            var lastEventPublishedReporter = new LastEventPublishedReporter();
            var wallet = new Wallet(lastEventPublishedReporter, new WalletIdentification(null), recurringChanges);

            wallet.ReportFundsOn(startDate.AddDays(27));

            var availableFundsOnDate = (AvailableFundsOnDate) lastEventPublishedReporter.LastEventPublished;

            Assert.That(availableFundsOnDate.Funds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Adding_funds_for_today_results_in_AvailableFunds_changing_immediately()
        {
            const decimal expectedFunds = 100;

            var today = DateTime.Today;
            var recurringChanges = new List<RecurringChange>
            {
                new RecurringChange(0, "Payday", 100m, new WeeklyRule(today, null))
            };
            var lastEventPublishedReporter = new LastEventPublishedReporter();
            var wallet = new Wallet(lastEventPublishedReporter, new WalletIdentification(null), recurringChanges);

            wallet.ReportFundsOn(today);

            var availableFundsOnDate = (AvailableFundsOnDate)lastEventPublishedReporter.LastEventPublished;

            Assert.That(availableFundsOnDate.Funds, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void StopChange_stops_change_from_day_after_LastApplicableDate()
        {
            var firstApplicableDate = new DateTime(1, 1, 1);
            var lastApplicableDate = new DateTime(2, 2, 2);
            const string changeName = "foo changeName";
            const int changeId = 111;

            var recurringChange = new RecurringChange(changeId, changeName, 123, new DailyRule(firstApplicableDate, null));
            Assert.That(recurringChange.AppliesTo(lastApplicableDate.AddDays(1)), "Does not apply to 1 day after last applicable date before being modified");
            var recurringChanges = new List<RecurringChange> { recurringChange };

            var wallet = new Wallet(MockRepository.GenerateStub<IReceivePublishedEvents>(), new WalletIdentification("foo name"), recurringChanges);
            wallet.StopChangeOn(changeId, lastApplicableDate);

            Assert.That(recurringChange.AppliesTo(lastApplicableDate));
            Assert.That(recurringChange.AppliesTo(lastApplicableDate.AddDays(1)), Is.False);
        }

        [Test]
        public void StopChange_raises_event_to_modify_change_entry_in_Database_when_last_date_is_different_to_first_date()
        {
            var firstApplicableDate = new DateTime(1, 1, 1);
            var lastApplicableDate = new DateTime(2, 2, 2);
            const string changeName = "foo changeName";
            const int changeId = 111;

            var recurringChange = new RecurringChange(changeId, changeName, 123, new DailyRule(firstApplicableDate, null));
            var recurringChanges = new List<RecurringChange> { recurringChange };

            var eventReciever = MockRepository.GenerateStub<IReceivePublishedEvents>();
            var wallet = new Wallet(eventReciever, new WalletIdentification("foo name"), recurringChanges);
            wallet.StopChangeOn(changeId, lastApplicableDate);

            eventReciever.AssertWasCalled(x => x.Publish(Arg<RecurringChangeModified>.Is.NotNull), c => c.Repeat.Once());
        }

        [Test]
        public void StopChange_raises_event_to_delete_change_entry_in_Database_when_last_date_is_the_same_as_to_first_date()
        {
            var firstApplicableDate = new DateTime(1, 1, 1);
            var lastApplicableDate = firstApplicableDate;
            const string changeName = "foo changeName";
            const int changeId = 111;

            var recurringChange = new RecurringChange(changeId, changeName, 123, new DailyRule(firstApplicableDate, null));
            var recurringChanges = new List<RecurringChange> { recurringChange };

            var eventReciever = MockRepository.GenerateStub<IReceivePublishedEvents>();
            var wallet = new Wallet(eventReciever, new WalletIdentification("foo name"), recurringChanges);
            wallet.StopChangeOn(changeId, lastApplicableDate);

            eventReciever.AssertWasCalled(x => x.Publish(Arg<RecurringChangeRemoved>.Is.NotNull), c => c.Repeat.Once());

            var argumentsForFirstCallToPublish = eventReciever.GetArgumentsForCallsMadeOn(x => x.Publish(Arg<AnEvent>.Is.Anything))[0];
            Assert.That(argumentsForFirstCallToPublish[0] is RecurringChangeRemoved);
        }

        [Test]
        public void RemoveChange_raises_event_to_delete_change_entry_in_Database()
        {
            var firstApplicableDate = new DateTime(1, 1, 1);
            const string changeName = "foo changeName";
            const int changeId = 111;

            var recurringChange = new RecurringChange(changeId, changeName, 123, new OneShotRule(firstApplicableDate, null));
            var recurringChanges = new List<RecurringChange> { recurringChange };

            var eventReciever = MockRepository.GenerateStub<IReceivePublishedEvents>();
            var wallet = new Wallet(eventReciever, new WalletIdentification("foo name"), recurringChanges);
            wallet.RemoveChange(changeId);

            eventReciever.AssertWasCalled(x => x.Publish(Arg<RecurringChangeRemoved>.Matches(m =>
                Equals(m.TargetIdentification, wallet.Identification))), c => c.Repeat.Once());
        }
    }
}