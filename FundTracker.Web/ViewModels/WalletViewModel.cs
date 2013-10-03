namespace FundTracker.Web.ViewModels
{
    public class WalletViewModel
    {
        public WalletViewModel(string name, decimal availableFunds)
        {
            AvailableFunds = availableFunds;
            Name = name;
        }

        public string Name { get; private set; }

        public decimal AvailableFunds { get; private set; }
    }
}