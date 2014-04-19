using System.Collections.Generic;
using FundTracker.Domain.Events;
using MicroEvent;

namespace FundTracker.Domain
{
    public class Wallet : IWallet
    {
        private readonly IReceivePublishedEvents _eventReciever;
        public List<RecurringChange> RecurringChanges { get; private set; }
        public WalletIdentification Identification { get; private set; }

        public decimal AvailableFunds { get; private set; }

        public Wallet(IReceivePublishedEvents eventReciever, WalletIdentification walletIdentification, decimal availableFunds, List<RecurringChange> recurringChanges)
        {
            _eventReciever = eventReciever;
            RecurringChanges = recurringChanges;
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
            RecurringChanges.Add(recurringChange);
            AddFunds(recurringChange.Amount);
            _eventReciever.Publish(new RecurringChangeCreated(recurringChange));
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
    }
}