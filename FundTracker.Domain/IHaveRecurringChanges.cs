using System;
using System.Collections.Generic;

namespace FundTracker.Domain
{
    public interface IHaveRecurringChanges : IAmIdentifiable
    {
        void CreateChange(RecurringChange recurringChange);
        IEnumerable<RecurringChange> GetChangesApplicableTo(DateTime selectedDate);
        void StopChangeOn(int changeId, DateTime lastApplicableDate);
        void RemoveChange(int changeId);
        int GetNextId();
    }
}