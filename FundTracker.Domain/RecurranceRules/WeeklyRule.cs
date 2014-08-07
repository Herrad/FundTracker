using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class WeeklyRule : RecurranceRule
    {

        public WeeklyRule(DateTime firstApplicableDate, DateTime? lastApplicableDate): base(firstApplicableDate, lastApplicableDate)
        {}

        protected override bool SpecificRulesApplyTo(DateTime targetDate)
        {
            return targetDate.DayOfWeek == FirstApplicableDate.DayOfWeek;
        }

        public override string PrettyPrint()
        {
            return "Every week starting " + FirstApplicableDate.ToString("dd MMM yyyy");
        }
    }
}