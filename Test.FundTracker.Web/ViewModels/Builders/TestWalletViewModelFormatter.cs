using System;
using System.Collections.Generic;
using FundTracker.Domain;
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

            var wallet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification(walletName), 0, new List<RecurringChange>());
            wallet.AddFunds(123m);
            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, new DateTime(1, 2, 3));

            Assert.That(result.Name, Is.EqualTo(walletName));
            Assert.That(result.AvailableFunds, Is.EqualTo(123m));
        }

        [Test]
        public void Sets_WithdrawalAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, 0, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange("foo", 123m, new DateTime(1, 2, 3)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, new DateTime(1, 2, 3));

            Assert.That(result.WithdrawalAmountViewModel, Is.Not.Null);
        }

        [Test]
        public void Only_uses_negative_changes_when_building_WithdrawalAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, 0, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange("foo", -50m, new DateTime(1, 2, 3)));
            wallet.CreateChange(new RecurringChange("foo", 100m, new DateTime(1, 2, 3)));
            wallet.CreateChange(new RecurringChange("foo", -25m, new DateTime(1, 2, 3)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, new DateTime(1, 2, 3));

            Assert.That(result.WithdrawalAmountViewModel.PositiveTotal, Is.EqualTo(75m));
        }

        [Test]
        public void Converts_RecurringChangeAmount_to_positive_when_negative_for_WithdrawalAmount()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new EventSwallower(), walletIdentification, 0, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange("foo", -50m, new DateTime(1, 2, 3)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, new DateTime(1, 2, 3));

            Assert.That(result.WithdrawalAmountViewModel.PositiveTotal, Is.EqualTo(50m));
        }

        [Test]
        public void Sets_DepositAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, 0, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange("foo", 123m, new DateTime(1, 2, 3)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, new DateTime(1, 2, 3));

            Assert.That(result.DepositAmountViewModel, Is.Not.Null);
        }

        [Test]
        public void Only_uses_positive_changes_when_building_DepositAmount_on_ViewModel()
        {
            const string walletName = "foo name";

            var viewModelFormatter = new WalletViewModelBuilder(new CalendarDayViewModelBuilder());

            var walletIdentification = new WalletIdentification(walletName);
            var wallet = new Wallet(new LastEventPublishedReporter(), walletIdentification, 0, new List<RecurringChange>());
            wallet.CreateChange(new RecurringChange("foo", -50m, new DateTime(1, 2, 3)));
            wallet.CreateChange(new RecurringChange("foo", 100m, new DateTime(1, 2, 3)));
            wallet.CreateChange(new RecurringChange("foo", -25m, new DateTime(1, 2, 3)));

            var result = viewModelFormatter.FormatWalletAsViewModel(wallet, new DateTime(1, 2, 3));

            Assert.That(result.DepositAmountViewModel.PositiveTotal, Is.EqualTo(100m));
        }
    }
}