using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class RecurranceSpecificationFactory : IBuildRecurranceSpecifications
    {
        public IDecideWhenRecurringChangesOccur Build(string aRecurranceRule, DateTime firstApplicableDate, DateTime? lastApplicableDate)
        {
            switch (aRecurranceRule)
            {
                case "Every week":
                    return new WeeklyRule(firstApplicableDate, lastApplicableDate);
                case "Every day":
                    return new DailyRule(firstApplicableDate, null);
                case "Just today":
                    return new OneShotRule(firstApplicableDate, null);
            }

            return new OneShotRule(firstApplicableDate, null);
        }
    }
}