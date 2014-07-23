using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class OneShotRule : RecurranceRule
    {
        public OneShotRule(DateTime firstApplicableDate, DateTime? lastApplicableDate) : base(firstApplicableDate, lastApplicableDate)
        {
        }

        protected override bool SpecificRulesApplyTo(DateTime targetDate)
        {
            return FirstApplicableDate == targetDate;
        }

        public override string Name { get { return "Just today"; } }
    }
}