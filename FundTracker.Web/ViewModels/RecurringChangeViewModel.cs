namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeViewModel
    {
        public string Name { get; private set; }
        public decimal Amount { get; private set; }

        public RecurringChangeViewModel(string name, decimal amount)
        {
            Amount = amount;
            Name = name;
        }
    }
}