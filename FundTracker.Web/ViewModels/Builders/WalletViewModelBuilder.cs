using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class WalletViewModelBuilder : IFormatWalletsAsViewModels
    {
        public WalletViewModel FormatWalletAsViewModel(Wallet wallet)
        {
            var walletViewModel = new WalletViewModel(wallet.Name, wallet.AvailableFunds);
            return walletViewModel;
        }
    }
}