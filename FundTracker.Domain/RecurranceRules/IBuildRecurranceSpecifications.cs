using System;

namespace FundTracker.Domain.RecurranceRules
{
    public interface IBuildRecurranceSpecifications
    {
        IDecideWhenRecurringChangesOccur Build(string aRecurranceRule, DateTime firstApplicableDate, DateTime? lastApplicableDate);
    }
}