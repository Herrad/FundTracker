using System;
using System.Collections.Generic;

namespace FundTracker.Domain
{
    public interface IHaveRecurringChanges : IAmIdentifiable
    {
        void CreateChange(RecurringChange recurringChange);
        IEnumerable<RecurringChange> GetRecurringDeposits();
        IEnumerable<RecurringChange> GetRecurringWithdrawals();
        IEnumerable<string> GetChangeNamesApplicableTo(DateTime selectedDate);
    }
}