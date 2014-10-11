using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class WalletViewModelBuilder : IFormatWalletsAsViewModels
    {
        private readonly IBuildWalletDatePickerViewModels _calendarDayViewModelBuilder;

        public WalletViewModelBuilder(IBuildWalletDatePickerViewModels calendarDayViewModelBuilder)
        {
            _calendarDayViewModelBuilder = calendarDayViewModelBuilder;
        }

        public WalletViewModel FormatWalletAsViewModel(IKnowAboutAvailableFunds wallet, DateTime selectedDate)
        {
            var walletIdentification = wallet.Identification;
            var walletName = walletIdentification.Name;

            var formattedSelectedDate = selectedDate.ToString("yyyy-MM-dd");
            var applicableChanges = wallet.GetChangesActiveOn(selectedDate).ToList();

            var recurringDepositAmount = GetTotalRecurringDepositAmount(applicableChanges);
            var depositAmountViewModel = new RecurringAmountViewModel("Deposit", recurringDepositAmount, walletName, formattedSelectedDate);

            var recurringWithdrawalsAmount = GetTotalRecurringWithdrawalsAmount(applicableChanges);
            var withdrawalAmountViewModel = new RecurringAmountViewModel("Withdrawal", recurringWithdrawalsAmount, walletName, formattedSelectedDate);

            var calendarDayViewModel = _calendarDayViewModelBuilder.Build(selectedDate, walletIdentification);

            var navigationLinkViewModels = BuildNavigationLinkViewModels(wallet, selectedDate);

            return new WalletViewModel(
                walletName, 
                wallet.GetAvailableFundsOn(selectedDate), 
                depositAmountViewModel, 
                withdrawalAmountViewModel, 
                calendarDayViewModel, 
                IsToday(selectedDate), 
                navigationLinkViewModels);
        }

        private static IEnumerable<NavigationLinkViewModel> BuildNavigationLinkViewModels(IAmIdentifiable wallet, DateTime selectedDate)
        {
            var navigationLinkViewModels = new List<NavigationLinkViewModel>();
            if (!IsToday(selectedDate))
            {
                navigationLinkViewModels.Add(new NavigationLinkViewModel("Jump to today",
                    "/Wallet/Display/?walletName=" + wallet.Identification.Name + "&date=" +
                    DateTime.Today.ToString("yyyy-MM-dd"), "go-to-today"));
            }
            return navigationLinkViewModels;
        }

        private static bool IsToday(DateTime selectedDate)
        {
            return selectedDate==DateTime.Today;
        }

        private static decimal GetTotalRecurringWithdrawalsAmount(IEnumerable<RecurringChange> applicableChanges)
        {
            return 0-applicableChanges.Where(change => change.Amount < 0).Sum(recurringChange => recurringChange.Amount);
        }

        private static decimal GetTotalRecurringDepositAmount(IEnumerable<RecurringChange> applicableChanges)
        {
            return applicableChanges.Where(change => change.Amount > 0).Sum(recurringChange => recurringChange.Amount);
        }
    }
}