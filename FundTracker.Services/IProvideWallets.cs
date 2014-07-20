using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IProvideWallets
    {
        IWallet FindFirstWalletWith(WalletIdentification walletIdentification);
        IHaveRecurringChanges FindRecurringChanger(WalletIdentification walletIdentification);
        IHaveChangingFunds FindFundChanger(WalletIdentification walletIdentification);
    }
}