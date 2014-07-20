namespace FundTracker.Domain
{
    public interface IStoreCreatedWallets
    {
        void Add(IHaveFundsThatFrequentlyChange wallet);
    }
}