namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeViewModel
    {
        public string Name { get; private set; }
        public decimal Amount { get; private set; }
        public string RuleType { get; private set; }
        public string StopLinkText { get; private set; }
        public int ChangeId { get; private set; }
        public string WalletName { get; private set; }

        public RecurringChangeViewModel(string name, decimal amount, string ruleType, string stopLinkText, string stopLinkDestination, int changeId, string walletName)
        {
            ChangeId = changeId;
            WalletName = walletName;
            StopLinkText = stopLinkText;
            RuleType = ruleType;
            Amount = amount;
            Name = name;
        }
    }
}