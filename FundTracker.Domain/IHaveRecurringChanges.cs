using System.Collections.Generic;

namespace FundTracker.Domain
{
    public interface IHaveRecurringChanges
    {
        List<RecurringChange> RecurringChanges { get; }
        void CreateChange(RecurringChange recurringChange);
    }
}