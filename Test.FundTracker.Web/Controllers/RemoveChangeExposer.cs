using System;
using System.Collections.Generic;
using FundTracker.Domain;

namespace Test.FundTracker.Web.Controllers
{
    public class RemoveChangeExposer : IHaveRecurringChanges
    {
        public WalletIdentification Identification { get; private set; }

        public int IdOfLastChangeRemoved { get; set; }

        public void CreateChange(RecurringChange recurringChange)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RecurringChange> GetChangesApplicableTo(DateTime selectedDate)
        {
            throw new NotImplementedException();
        }

        public void StopChangeOn(int changeId, DateTime lastApplicableDate)
        {
            throw new NotImplementedException();
        }

        public void RemoveChange(int changeId)
        {
            IdOfLastChangeRemoved = changeId;
        }

        public int GetNextId()
        {
            throw new NotImplementedException();
        }
    }
}