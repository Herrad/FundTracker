using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class RecurranceSpecificationFactory : IBuildRecurranceSpecifications
    {
        public IDecideWhenRecurringChangesOccur Build(string aRecurranceRule, DateTime firstApplicableDate)
        {
            switch (aRecurranceRule)
            {
                case "Every week":
                    return new WeeklyRule(firstApplicableDate);
                case "Every day":
                    return new DailyRule(firstApplicableDate);
                case "Just today":
                    return new OneShotRule(firstApplicableDate);
            }

            return new OneShotRule(firstApplicableDate);
        }
    }
}