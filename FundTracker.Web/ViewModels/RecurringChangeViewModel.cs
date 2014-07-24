using System;

namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeViewModel
    {
        public string Name { get; private set; }
        public decimal Amount { get; private set; }
        public DateTime AppliedSince { get; private set; }
        public string RuleType { get; private set; }
        public string StopLinkText { get; private set; }
        public string StopLinkDestination { get; private set; }

        public RecurringChangeViewModel(string name, decimal amount, DateTime appliedSince, string ruleType, string stopLinkText, string stopLinkDestination)
        {
            StopLinkDestination = stopLinkDestination;
            StopLinkText = stopLinkText;
            RuleType = ruleType;
            AppliedSince = appliedSince;
            Amount = amount;
            Name = name;
        }
    }
}