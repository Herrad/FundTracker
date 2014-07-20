using System;
using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class WalletViewModelBuilder : IFormatWalletsAsViewModels
    {
        private readonly IBuildCalanderDayViewModels _calendarDayViewModelBuilder;

        public WalletViewModelBuilder(IBuildCalanderDayViewModels calendarDayViewModelBuilder)
        {
            _calendarDayViewModelBuilder = calendarDayViewModelBuilder;
        }

        public WalletViewModel FormatWalletAsViewModel(IHaveRecurringChanges wallet, IHaveChangingFunds fundChanger, DateTime selectedDate)
        {
            var formattedSelectedDate = selectedDate.ToString("yyyy-MM-dd");
            var depositAmountViewModel = BuildDepositAmountViewModel(wallet, formattedSelectedDate);
            var withdrawalAmountViewModel = BuildWithdrawalAmountViewModel(wallet, formattedSelectedDate);

            var calendarDayViewModel = _calendarDayViewModelBuilder.Build(selectedDate, wallet.Identification);

            return new WalletViewModel(wallet.Identification.Name, fundChanger.GetAvailableFundsFor(selectedDate), depositAmountViewModel, withdrawalAmountViewModel, calendarDayViewModel);
        }

        private static RecurringAmountViewModel BuildDepositAmountViewModel(IHaveRecurringChanges wallet, string date)
        {
            var recurringDeposits = wallet.GetRecurringDeposits();
            var depositAmountViewModel = new RecurringAmountViewModel("Deposit",
                recurringDeposits.Sum(recurringChange => recurringChange.Amount), wallet.Identification.Name, date);
            return depositAmountViewModel;
        }

        private static RecurringAmountViewModel BuildWithdrawalAmountViewModel(IHaveRecurringChanges wallet, string date)
        {
            var recurringWithdrawals = wallet.GetRecurringWithdrawals();
            var withdrawalAmountViewModel = new RecurringAmountViewModel("Withdrawal",
                recurringWithdrawals.Sum(recurringChange => 0 - recurringChange.Amount), wallet.Identification.Name, date);
            return withdrawalAmountViewModel;
        }
    }
}