namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeViewModel
    {
        public string Name { get; private set; }
        public decimal Amount { get; private set; }
        public string RuleType { get; private set; }
        public string StopLinkText { get; private set; }
        public string StopLinkDestination { get; private set; }
        public int ChangeId { get; private set; }

        public RecurringChangeViewModel(string name, decimal amount, string ruleType, string stopLinkText, string stopLinkDestination, int changeId)
        {
            ChangeId = changeId;
            StopLinkDestination = stopLinkDestination;
            StopLinkText = stopLinkText;
            RuleType = ruleType;
            Amount = amount;
            Name = name;
        }
    }
}