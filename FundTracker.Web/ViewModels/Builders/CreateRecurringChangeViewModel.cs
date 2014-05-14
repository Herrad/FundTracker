namespace FundTracker.Web.ViewModels.Builders
{
    public class CreateRecurringChangeViewModel
    {
        public CreateRecurringChangeViewModel(string walletName)
        {
            WalletName = walletName;
        }

        public string WalletName { get; private set; }
    }
}