using System;
using System.Collections.Generic;

namespace FundTracker.Domain
{
    public interface IHaveRecurringChanges : IAmIdentifiable
    {
        void CreateChange(RecurringChange recurringChange);
        IEnumerable<RecurringChange> GetChangesApplicableTo(DateTime selectedDate);
        void StopChangeOn(string changeName, DateTime lastApplicableDate);
        void RemoveChange(string changeName);
    }
}