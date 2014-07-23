using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Domain.Events;
using FundTracker.Domain.RecurranceRules;
using NUnit.Framework;

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
            var recurringChanges = new List<RecurringChange>()
            {
                new RecurringChange("Payday", 100m, startDate, new WeeklyRule(startDate, null))
            };
            var wallet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification(null), recurringChanges);

            var availableFundsInFourWeeks = wallet.GetAvailableFundsFor(startDate.AddDays(27));

            Assert.That(availableFundsInFourWeeks, Is.EqualTo(expectedFunds));
        }

        [Test]
        public void Adding_funds_for_today_results_in_AvailableFunds_changing_immediately()
        {
            const decimal expectedFunds = 100;

            var today = DateTime.Today;
            var recurringChanges = new List<RecurringChange>()
            {
                new RecurringChange("Payday", 100m, today, new WeeklyRule(today, null))
            };
            var wallet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification(null), recurringChanges);

            var availableFundsInFourWeeks = wallet.GetAvailableFundsFor(today);

            Assert.That(availableFundsInFourWeeks, Is.EqualTo(expectedFunds));
        }
    }
}