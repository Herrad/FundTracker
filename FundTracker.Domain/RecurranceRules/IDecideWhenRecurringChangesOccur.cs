using System;

namespace FundTracker.Domain.RecurranceRules
{
    public interface IDecideWhenRecurringChangesOccur
    {
        bool IsSpecifiedOn(DateTime targetDate);

        DateTime FirstApplicableDate { get; }
        DateTime? LastApplicableDate { get; }
        void StopOn(DateTime lastApplicableDate);

        string PrettyPrint();
    }
}