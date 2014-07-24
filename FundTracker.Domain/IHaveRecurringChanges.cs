using System;
using System.Collections.Generic;
using FundTracker.Domain.RecurranceRules;

namespace FundTracker.Domain
{
    public interface IHaveRecurringChanges : IAmIdentifiable
    {
        IEnumerable<RecurringChange> GetChangesActiveOn(DateTime selectedDate);
        void StopChangeOn(int changeId, DateTime lastApplicableDate);
        void RemoveChange(int changeId);
        void CreateChange(string changeName, decimal amount, IDecideWhenRecurringChangesOccur recurranceSpecification);
    }
}