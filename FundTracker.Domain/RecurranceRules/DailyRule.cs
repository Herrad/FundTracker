using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class DailyRule : IDecideWhenRecurringChangesOccur
    {
        private readonly DateTime _firstApplicableDate;

        public DailyRule(DateTime firstApplicableDate)
        {
            _firstApplicableDate = firstApplicableDate;
        }

        public bool IsSpecifiedOn(DateTime targetDate)
        {
            return targetDate >= _firstApplicableDate;
        }

        public string Name { get { return "Every day"; } }
    }
}