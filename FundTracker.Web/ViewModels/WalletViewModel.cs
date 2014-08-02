using System.Collections.Generic;
using System.Web.Mvc;

namespace FundTracker.Web.ViewModels
{
    public class WalletViewModel
    {
        public WalletViewModel(string name, decimal availableFunds, RecurringAmountViewModel depositAmountViewModel, RecurringAmountViewModel withdrawalAmountViewModel, WalletDatePickerViewModel dayViewModel, bool displayQuickChanges, IEnumerable<NavigationLinkViewModel> navigationLinks)
        {
            NavigationLinks = navigationLinks;
            DisplayQuickChanges = displayQuickChanges;
            DayViewModel = dayViewModel;
            WithdrawalAmountViewModel = withdrawalAmountViewModel;
            AvailableFunds = availableFunds;
            Name = name;
            DepositAmountViewModel = depositAmountViewModel;
        }

        public string Name { get; private set; }

        public decimal AvailableFunds { get; private set; }

        public RecurringAmountViewModel DepositAmountViewModel { get; private set; }
        public RecurringAmountViewModel WithdrawalAmountViewModel { get; private set; }
        public WalletDatePickerViewModel DayViewModel { get; private set; }
        public bool DisplayQuickChanges { get; private set; }
        public IEnumerable<NavigationLinkViewModel> NavigationLinks { get; private set; }
    }
}