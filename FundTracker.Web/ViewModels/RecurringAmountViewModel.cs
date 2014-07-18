namespace FundTracker.Web.ViewModels
{
    public class RecurringAmountViewModel
    {
        public RecurringAmountViewModel(string recurringType, decimal positiveTotal, string walletName, string date)
        {
            Date = date;
            WalletName = walletName;
            PositiveTotal = positiveTotal;
            RecurringType = recurringType;
        }

        public string RecurringType { get; private set; }
        public decimal PositiveTotal { get; private set; }
        public string WalletName { get; private set; }
        public string Date { get; private set; }
    }
}