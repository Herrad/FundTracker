using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class DailyRule : RecurranceRule
    {
        private readonly DateTime _firstApplicableDate;

        public DailyRule(DateTime firstApplicableDate, DateTime? lastApplicableDate) : base(firstApplicableDate, lastApplicableDate)
        {
            _firstApplicableDate = firstApplicableDate;
        }

        protected override bool SpecificRulesApplyTo(DateTime targetDate)
        {
            return targetDate >= _firstApplicableDate;
        }

        public override string Name { get { return "Every day"; } }
    }
}