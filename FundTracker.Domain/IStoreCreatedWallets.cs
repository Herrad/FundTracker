namespace FundTracker.Domain
{
    public interface IStoreCreatedWallets
    {
        void Add(IAmIdentifiable wallet);
    }
}