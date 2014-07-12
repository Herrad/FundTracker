namespace FundTracker.Domain
{
    public class RecurringChange
    {
        public RecurringChange(string name, decimal amount)
        {
            Name = name;
            Amount = amount;
        }

        public string Name { get; private set; }
        public decimal Amount { get; private set; }
    }
}