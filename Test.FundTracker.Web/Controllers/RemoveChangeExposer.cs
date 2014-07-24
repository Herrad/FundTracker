using System;
using System.Collections.Generic;
using FundTracker.Domain;

namespace Test.FundTracker.Web.Controllers
{
    public class RemoveChangeExposer : IHaveRecurringChanges
    {
        public WalletIdentification Identification { get; private set; }
        public string NameOfLastChangeRemoved { get; private set; }

        public void CreateChange(RecurringChange recurringChange)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RecurringChange> GetChangesApplicableTo(DateTime selectedDate)
        {
            throw new NotImplementedException();
        }

        public void StopChangeOn(string changeName, DateTime lastApplicableDate)
        {
            throw new NotImplementedException();
        }

        public void RemoveChange(string changeName)
        {
            NameOfLastChangeRemoved = changeName;
        }
    }
}