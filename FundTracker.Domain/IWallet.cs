namespace FundTracker.Domain
{
    public interface IWallet : ITakeFundsToAdd, IHaveAvailableFunds, IHaveRecurringChanges
    {
        WalletIdentification Identification { get; }
    }
}