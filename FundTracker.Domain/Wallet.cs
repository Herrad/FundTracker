using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain.Events;
using FundTracker.Domain.RecurranceRules;
using MicroEvent;

namespace FundTracker.Domain
{
    public class Wallet : IKnowAboutAvailableFunds, IHaveRecurringChanges
    {
        private readonly IReceivePublishedEvents _eventReciever;
        private readonly List<RecurringChange> _recurringChanges;
        private static readonly DateTime EarliestChangeDate = new DateTime(2013, 01, 01);

        public WalletIdentification Identification { get; private set; }

        public Wallet(IReceivePublishedEvents eventReciever, WalletIdentification walletIdentification, List<RecurringChange> recurringChanges)
        {
            _eventReciever = eventReciever;
            _recurringChanges = recurringChanges;
            Identification = walletIdentification;
        }

        private int GetNextId()
        {
            if (!_recurringChanges.Any())
            {
                return 1;
            }
            return _recurringChanges.OrderByDescending(x => x.Id).First().Id + 1;
        }

        public decimal GetAvailableFundsOn(DateTime targetDate)
        {
            var runningTotal = 0m;
            var differenceBetweenTargetDateAndEarliest = (targetDate - EarliestChangeDate).Days;
            var remainingDifferenceInDays = differenceBetweenTargetDateAndEarliest;
            while (remainingDifferenceInDays >= 0)
            {
                var invertedDifference = 0 - remainingDifferenceInDays;
                var recurringChangeQuery = targetDate.AddDays(invertedDifference);
                var applicableChanges = GetChangesActiveOn(recurringChangeQuery);

                runningTotal += applicableChanges.Sum(x => x.Amount);

                remainingDifferenceInDays--;
            }
            return runningTotal;
        }

        public void CreateChange(string changeName, decimal amount,IDecideWhenRecurringChangesOccur recurranceSpecification)
        {
            var recurringChange = new RecurringChange(GetNextId(), changeName, amount, recurranceSpecification);
            _recurringChanges.Add(recurringChange);
            _eventReciever.Publish(new RecurringChangeCreated(recurringChange, Identification, recurringChange.ToValues()));
        }

        public IEnumerable<RecurringChange> GetChangesActiveOn(DateTime selectedDate)
        {
            return _recurringChanges.Where(recurringChange => recurringChange.AppliesTo(selectedDate));
        }

        public void StopChangeOn(int changeId, DateTime lastApplicableDate)
        {
            var recurringChange = _recurringChanges.First(change => change.Id == changeId);
            recurringChange.StopOn(lastApplicableDate);
            if (recurringChange.CanBeDeleted())
            {
                _eventReciever.Publish(new RecurringChangeRemoved(Identification, recurringChange));
            }
            else
            {
                _eventReciever.Publish(new RecurringChangeModified(Identification, recurringChange.ToValues()));
            }
        }

        public void RemoveChange(int changeId)
        {
            StopChangeOn(changeId, DateTime.Today);
            _eventReciever.Publish(new RecurringChangeRemoved(Identification, _recurringChanges.First(change => change.Id == changeId)));
        }

        private bool Equals(IAmIdentifiable other)
        {
            return Identification.Equals(other.Identification);
        }

        public override int GetHashCode()
        {
            return (Identification!= null ? Identification.GetHashCode() : 0);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Wallet) obj);
        }
    }
}