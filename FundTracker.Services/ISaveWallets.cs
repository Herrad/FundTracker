using FundTracker.Domain;

namespace FundTracker.Services
{
    public interface ISaveWallets
    {
        void Save(IWallet wallet);
    }
}