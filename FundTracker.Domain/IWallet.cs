namespace FundTracker.Domain
{
    public interface IWallet : ITakeFundsToAdd, IHaveAvailableFunds
    {
        string Name { get; }
    }
}