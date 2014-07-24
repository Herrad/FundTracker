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

        public void StopChangeOn(string changeName, DateTime lastApplicableDate)
        {
            throw new NotImplementedException();
        }

        public void RemoveChange(string changeName)
        {
            throw new NotImplementedException();
        }

        public RecurringChange LastChangeAdded { get; private set; }
    }
}