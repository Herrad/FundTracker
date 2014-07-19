using System;

namespace FundTracker.Domain.RecurranceRules
{
    public interface IDecideWhenRecurringChangesOccur
    {
        bool IsSpecifiedOn(DateTime targetDate);
        string Name { get; }
    }
}