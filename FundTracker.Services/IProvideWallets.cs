using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IProvideWallets
    {
        IHaveRecurringChanges FindRecurringChanger(WalletIdentification walletIdentification);
        IKnowAboutAvailableFunds FindFundChanger(WalletIdentification walletIdentification);
    }
}