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
        public void Sets_WithdrawalAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, new List<RecurringChange>());
            var dateToApplyTo = new DateTime(1, 2, 3);
            wallet.CreateChange("foo", 123m, new OneShotRule(dateToApplyTo, null));

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
            var oneShotRule = new OneShotRule(dateToApplyTo, null);
            wallet.CreateChange("foo", -50m, oneShotRule);
            wallet.CreateChange("foo", 100m, oneShotRule);
            wallet.CreateChange("foo", -25m, oneShotRule);

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
            wallet.CreateChange("foo", -50m, new OneShotRule(dateToApplyTo, null));

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
            wallet.CreateChange("foo", 123m, new OneShotRule(selectedDate, null));

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
            wallet.CreateChange("foo", -50m, new OneShotRule(selectedDate, null));
            wallet.CreateChange("foo", 100m, new OneShotRule(selectedDate, null));
            wallet.CreateChange("foo", -25m, new OneShotRule(selectedDate, null));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, selectedDate);

            Assert.That(result.DepositAmountViewModel.PositiveTotal, Is.EqualTo(100m));
        }

        [Test]
        public void Displays_quick_changes_when_selected_date_is_Today()
        {
            const string walletName = "foo name";
            var walletIdentification = new WalletIdentification(walletName);

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, new List<RecurringChange>());

            var selectedDate = DateTime.Today;
            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, selectedDate);


            Assert.That(result.DisplayQuickChanges, Is.True);
        }

        [Test]
        public void Does_not_display_quick_changes_when_selected_date_is_not_Today()
        {
            const string walletName = "foo name";
            var walletIdentification = new WalletIdentification(walletName);

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, new List<RecurringChange>());

            var selectedDate = DateTime.Today.AddDays(1);
            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, wallet, selectedDate);


            Assert.That(result.DisplayQuickChanges, Is.False);
        }


    }
}