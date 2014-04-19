namespace FundTracker.Domain
{
    public interface IStoreCreatedWallets
    {
        void Add(IWallet wallet);
    }
}