using System;

namespace FundTracker.Domain.RecurranceRules
{
    public interface IBuildRecurranceSpecifications
    {
        IDecideWhenRecurringChangesOccur Build(string ruleType, DateTime firstApplicableDate, DateTime? lastApplicableDate);
    }
}