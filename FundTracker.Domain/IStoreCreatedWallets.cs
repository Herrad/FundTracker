namespace FundTracker.Domain
{
    public interface IStoreCreatedWallets
    {
        void Add(IHaveChangingFunds wallet);
    }
}