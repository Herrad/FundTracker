using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain.Events;
using FundTracker.Domain.RecurranceRules;
using MicroEvent;

namespace FundTracker.Domain
{
    public class Wallet : IHaveChangingFunds, IHaveRecurringChanges
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

        public void AddFunds(decimal fundsToAdd)
        {
            CreateChange(new RecurringChange("AD-HOC CHANGE", fundsToAdd, new OneShotRule(DateTime.Today, null)));
        }

        public void CreateChange(RecurringChange recurringChange)
        {
            _recurringChanges.Add(recurringChange);
            _eventReciever.Publish(new RecurringChangeCreated(recurringChange, Identification));
        }

        public decimal GetAvailableFundsFor(DateTime targetDate)
        {
            var runningTotal = 0m;
            var differenceBetweenTargetDateAndEarliest = (targetDate - EarliestChangeDate).Days;
            var remainingDifference = differenceBetweenTargetDateAndEarliest;
            while (remainingDifference >= 0)
            {
                var invertedDifference = 0 - remainingDifference;
                var recurringChangeQuery = targetDate.AddDays(invertedDifference);
                var applicableChanges = GetChangesApplicableTo(recurringChangeQuery);

                runningTotal += applicableChanges.Sum(x => x.Amount);

                remainingDifference--;
            }
            return runningTotal;
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

        public IEnumerable<RecurringChange> GetChangesApplicableTo(DateTime selectedDate)
        {
            return _recurringChanges.Where(recurringChange => recurringChange.AppliesTo(selectedDate));
        }

        public void StopChangeOn(string changeName, DateTime lastApplicableDate)
        {
            _recurringChanges.First(change => change.Name == changeName).StopOn(lastApplicableDate);
        }
    }
}