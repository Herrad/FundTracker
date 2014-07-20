using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Test.FundTracker.Domain;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestWalletViewModelFormatter
    {
        [Test]
        public void Sets_WalletName_and_Amount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var wallet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification(walletName), new List<RecurringChange>());
            wallet.AddFunds(123m);
            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, DateTime.Today);

            Assert.That(result.Name, Is.EqualTo(walletName));
            Assert.That(result.AvailableFunds, Is.EqualTo(123m));
        }

        [Test]
        public void Sets_WithdrawalAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, new List<RecurringChange>());
            var dateToApplyTo = new DateTime(1, 2, 3);
            wallet.CreateChange(new RecurringChange("foo", 123m, dateToApplyTo, new OneShotRule(dateToApplyTo)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, dateToApplyTo);

            Assert.That(result.WithdrawalAmountViewModel, Is.Not.Null);
        }

        [Test]
        public void Only_uses_negative_changes_when_building_WithdrawalAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, new List<RecurringChange>());
            var dateToApplyTo = new DateTime(1, 2, 3);
            var oneShotRule = new OneShotRule(dateToApplyTo);
            wallet.CreateChange(new RecurringChange("foo", -50m, dateToApplyTo, oneShotRule));
            wallet.CreateChange(new RecurringChange("foo", 100m, dateToApplyTo, oneShotRule));
            wallet.CreateChange(new RecurringChange("foo", -25m, dateToApplyTo, oneShotRule));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, dateToApplyTo);

            Assert.That(result.WithdrawalAmountViewModel.PositiveTotal, Is.EqualTo(75m));
        }

        [Test]
        public void Converts_RecurringChangeAmount_to_positive_when_negative_for_WithdrawalAmount()
        {
            const string walletName = "foo name";
            var dateToApplyTo = new DateTime(1, 2, 3);

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new EventSwallower(), walletIdentification, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange("foo", -50m, dateToApplyTo, new OneShotRule(dateToApplyTo)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, dateToApplyTo);

            Assert.That(result.WithdrawalAmountViewModel.PositiveTotal, Is.EqualTo(50m));
        }

        [Test]
        public void Sets_DepositAmount_on_ViewModel()
        {
            const string walletName = "foo name";
            var selectedDate = new DateTime(1, 2, 3);

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange("foo", 123m, selectedDate, new OneShotRule(selectedDate)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, selectedDate);

            Assert.That(result.DepositAmountViewModel, Is.Not.Null);
        }

        [Test]
        public void Only_uses_positive_changes_when_building_DepositAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, new List<RecurringChange>());
            var selectedDate = new DateTime(1, 2, 3);
            wallet.CreateChange(new RecurringChange("foo", -50m, selectedDate, new OneShotRule(selectedDate)));
            wallet.CreateChange(new RecurringChange("foo", 100m, selectedDate, new OneShotRule(selectedDate)));
            wallet.CreateChange(new RecurringChange("foo", -25m, selectedDate, new OneShotRule(selectedDate)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, selectedDate);

            Assert.That(result.DepositAmountViewModel.PositiveTotal, Is.EqualTo(100m));
        }


    }
}