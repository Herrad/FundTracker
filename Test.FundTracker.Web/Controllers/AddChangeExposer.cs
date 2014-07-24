using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;

namespace Test.FundTracker.Web.Controllers
{
    public class AddChangeExposer : IHaveRecurringChanges
    {
        WalletIdentification IAmIdentifiable.Identification { get { return null; } }

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

        public void CreateChange(string changeName, decimal amount,
            IDecideWhenRecurringChangesOccur recurranceSpecification)
        {
            throw new NotImplementedException();
        }

        public RecurringChange LastChangeAdded { get; private set; }
    }
}