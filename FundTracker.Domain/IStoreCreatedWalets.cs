namespace FundTracker.Domain
{
    public interface IStoreCreatedWalets
    {
        void Add(IWallet wallet);
    }
}