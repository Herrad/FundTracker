using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface IKnowAboutWallets
    {
        Wallet Get(WalletIdentification identification);
    }
}