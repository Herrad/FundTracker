using System;

namespace FundTracker.Domain.RecurranceRules
{
    public interface IDecideWhenRecurringChangesOccur
    {
        bool IsSpecifiedOn(DateTime targetDate);

        string Name { get; }
        DateTime FirstApplicableDate { get; }
        DateTime? LastApplicableDate { get; }
        void StopOn(DateTime lastApplicableDate);
    }
}