﻿using System;
using System.Collections.Generic;
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
            var applicableChanges = wallet.GetChangesApplicableTo(selectedDate).ToList();

            var recurringDepositAmount = GetTotalRecurringDepositAmount(applicableChanges);
            var depositAmountViewModel = new RecurringAmountViewModel("Deposit",recurringDepositAmount, wallet.Identification.Name, formattedSelectedDate);

            var recurringWithdrawalsAmount = GetTotalRecurringWithdrawalsAmount(applicableChanges);
            var withdrawalAmountViewModel = new RecurringAmountViewModel("Withdrawal", recurringWithdrawalsAmount, wallet.Identification.Name, formattedSelectedDate);

            var calendarDayViewModel = _calendarDayViewModelBuilder.Build(selectedDate, wallet.Identification);

            return new WalletViewModel(wallet.Identification.Name, fundChanger.GetAvailableFundsFor(selectedDate), depositAmountViewModel, withdrawalAmountViewModel, calendarDayViewModel);
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