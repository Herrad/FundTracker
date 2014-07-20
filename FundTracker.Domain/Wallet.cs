using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain.Events;
using MicroEvent;

namespace FundTracker.Domain
{
    public class Wallet : IWallet
    {
        private readonly IReceivePublishedEvents _eventReciever;
        private readonly List<RecurringChange> _recurringChanges;

        public WalletIdentification Identification { get; private set; }

        public decimal AvailableFunds { get; private set; }

        public Wallet(IReceivePublishedEvents eventReciever, WalletIdentification walletIdentification, decimal availableFunds, List<RecurringChange> recurringChanges)
        {
            _eventReciever = eventReciever;
            _recurringChanges = recurringChanges;
            Identification = walletIdentification;
            AvailableFunds = availableFunds;
        }

        public void AddFunds(decimal fundsToAdd)
        {
            AvailableFunds += fundsToAdd;
            _eventReciever.Publish(new WalletFundsChanged(this));
        }

        public void CreateChange(RecurringChange recurringChange)
        {
            _recurringChanges.Add(recurringChange);
            AddFunds(recurringChange.Amount);
            _eventReciever.Publish(new RecurringChangeCreated(recurringChange, Identification));
        }

        protected bool Equals(Wallet other)
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

        public IEnumerable<RecurringChange> GetRecurringDeposits()
        {
            return _recurringChanges.Where(recurringChange => recurringChange.Amount > 0);
        }

        public IEnumerable<RecurringChange> GetRecurringWithdrawals()
        {
            return _recurringChanges.Where(recurringChange => recurringChange.Amount < 0);
        }

        public IEnumerable<string> GetChangeNamesApplicableTo(DateTime selectedDate)
        {
            return _recurringChanges.Where(x => x.AppliesTo(selectedDate)).Select(x => x.Name);
        }
    }
}