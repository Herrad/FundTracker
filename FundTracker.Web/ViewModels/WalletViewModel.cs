namespace FundTracker.Web.ViewModels
{
    public class WalletViewModel
    {
        public WalletViewModel(string name, decimal availableFunds, RecurringAmountViewModel depositAmountViewModel, RecurringAmountViewModel withdrawalAmountViewModel)
        {
            WithdrawalAmountViewModel = withdrawalAmountViewModel;
            AvailableFunds = availableFunds;
            Name = name;
            DepositAmountViewModel = depositAmountViewModel;
        }

        public string Name { get; private set; }

        public decimal AvailableFunds { get; private set; }

        public RecurringAmountViewModel DepositAmountViewModel { get; private set; }
        public RecurringAmountViewModel WithdrawalAmountViewModel { get; private set; }
    }
}