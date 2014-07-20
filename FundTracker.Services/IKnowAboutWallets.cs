using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IKnowAboutWallets
    {
        IHaveFundsThatFrequentlyChange Get(WalletIdentification identification);
    }
}