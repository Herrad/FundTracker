using System;
using System.Collections.Generic;

namespace FundTracker.Domain
{
    public interface IHaveRecurringChanges
    {
        void CreateChange(RecurringChange recurringChange);
        IEnumerable<RecurringChange> GetRecurringDeposits();
        IEnumerable<RecurringChange> GetRecurringWithdrawals();
        IEnumerable<string> GetChangeNamesApplicableTo(DateTime selectedDate);
    }
}