using System;
using System.Collections.Generic;
using FundTracker.Domain;

namespace Test.FundTracker.Web.Controllers
{
    public class AddChangeExposer : IHaveRecurringChanges
    {
        WalletIdentification IAmIdentifiable.Identification { get { return null; } }

        public void CreateChange(RecurringChange recurringChange)
        {
            LastChangeAdded = recurringChange;
        }

        IEnumerable<RecurringChange> IHaveRecurringChanges.GetChangesApplicableTo(DateTime selectedDate)
        {
            throw new NotImplementedException();
        }

        public void StopChangeOn(int changeId, DateTime lastApplicableDate)
        {
            throw new NotImplementedException();
        }

        public void RemoveChange(int changeId)
        {
            throw new NotImplementedException();
        }

        public int GetNextId()
        {
            throw new NotImplementedException();
        }

        public RecurringChange LastChangeAdded { get; private set; }
    }
}