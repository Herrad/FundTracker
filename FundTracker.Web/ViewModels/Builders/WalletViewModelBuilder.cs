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

        public WalletViewModel FormatWalletAsViewModel(IWallet wallet, DateTime selectedDate)
        {
            var depositAmountViewModel = BuildDepositAmountViewModel(wallet);
            var withdrawalAmountViewModel = BuildWithdrawalAmountViewModel(wallet);

            var calendarDayViewModel = _calendarDayViewModelBuilder.Build(selectedDate, wallet.Identification);

            return new WalletViewModel(wallet.Identification.Name, wallet.AvailableFunds, depositAmountViewModel, withdrawalAmountViewModel, calendarDayViewModel);
        }

        private static RecurringAmountViewModel BuildDepositAmountViewModel(IHaveRecurringChanges wallet)
        {
            var recurringDeposits = wallet.RecurringChanges.Where(recurringChange => recurringChange.Amount > 0);
            var depositAmountViewModel = new RecurringAmountViewModel("Deposit",
                recurringDeposits.Sum(recurringChange => recurringChange.Amount));
            return depositAmountViewModel;
        }

        private static RecurringAmountViewModel BuildWithdrawalAmountViewModel(IHaveRecurringChanges wallet)
        {
            var recurringWithdrawals = wallet.RecurringChanges.Where(recurringChange => recurringChange.Amount < 0);
            var withdrawalAmountViewModel = new RecurringAmountViewModel("Withdrawal",
                recurringWithdrawals.Sum(recurringChange => 0 - recurringChange.Amount));
            return withdrawalAmountViewModel;
        }
    }
}