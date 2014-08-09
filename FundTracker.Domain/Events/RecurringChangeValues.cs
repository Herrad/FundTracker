namespace FundTracker.Domain.Events
{
    public class RecurringChangeValues
    {
        public decimal Amount { get; private set; }
        public string Name { get; private set; }
        public string StartDate { get; private set; }
        public string EndDate { get; private set; }
        public string RuleType { get; private set; }
        public int Id { get; private set; }

        public RecurringChangeValues(decimal amount, string name, string startDate, string endDate, string ruleType, int id)
        {
            Amount = amount;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            RuleType = ruleType;
            Id = id;
        }
    }
}