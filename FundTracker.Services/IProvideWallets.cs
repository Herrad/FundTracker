using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IProvideWallets
    {
        IWallet FindFirstWalletWith(WalletIdentification walletIdentification);
    }
}