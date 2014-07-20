using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class WeeklyRule : IDecideWhenRecurringChangesOccur
    {
        private readonly DateTime _firstApplicableDate;

        public WeeklyRule(DateTime firstApplicableDate)
        {
            _firstApplicableDate = firstApplicableDate;
        }

        public bool IsSpecifiedOn(DateTime targetDate)
        {
            return targetDate >= _firstApplicableDate && targetDate.DayOfWeek == _firstApplicableDate.DayOfWeek;
        }

        public string Name { get { return "Every week"; } }
    }
}