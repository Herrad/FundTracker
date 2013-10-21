namespace FundTracker.Web.ViewModels.Builders
{
    public class CreateWithdrawalViewModel
    {
        public CreateWithdrawalViewModel(string walletName)
        {
            WalletName = walletName;
        }

        public string WalletName { get; private set; }
    }
}