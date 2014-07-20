using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IProvideWallets
    {
        IHaveRecurringChanges FindRecurringChanger(WalletIdentification walletIdentification);
        IHaveChangingFunds FindFundChanger(WalletIdentification walletIdentification);
    }
}