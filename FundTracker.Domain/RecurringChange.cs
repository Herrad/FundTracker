namespace FundTracker.Domain
{
    public class RecurringChange
    {
        public RecurringChange(WalletIdentification identification, decimal amount)
        {
            Identification = identification;
            Amount = amount;
        }

        public decimal Amount { get; private set; }
        public WalletIdentification Identification { get; private set; }
    }
}