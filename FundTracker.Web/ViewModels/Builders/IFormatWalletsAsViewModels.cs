using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IFormatWalletsAsViewModels
    {
        WalletViewModel FormatWalletAsViewModel(Wallet wallet);
    }
}