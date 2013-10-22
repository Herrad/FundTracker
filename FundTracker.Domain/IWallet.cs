namespace FundTracker.Domain
{
    public interface IWallet : ITakeFundsToAdd, IHaveAvailableFunds
    {
        WalletIdentification Identification { get; }
    }
}