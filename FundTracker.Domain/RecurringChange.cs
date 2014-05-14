namespace FundTracker.Domain
{
    public class RecurringChange
    {
        public RecurringChange(WalletIdentification targetWalletIdentifier, decimal amount)
        {
            TargetWalletIdentifier = targetWalletIdentifier;
            Amount = amount;
        }

        public decimal Amount { get; private set; }
        public WalletIdentification TargetWalletIdentifier { get; private set; }
    }
}