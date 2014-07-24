using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;

namespace Test.FundTracker.Web.Controllers
{
    public class RemoveChangeExposer : IHaveRecurringChanges
    {
        public WalletIdentification Identification { get; private set; }

        public int IdOfLastChangeRemoved { get; private set; }

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

        public void CreateChange(string changeName, decimal amount,
            IDecideWhenRecurringChangesOccur recurranceSpecification)
        {
            throw new NotImplementedException();
        }
    }
}