using System.Collections.Generic;
using FundTracker.Domain;

namespace Test.FundTracker.Web.Controllers
{
    public class WithdrawalExposer : IWallet
    {
        public void AddFunds(decimal fundsToAdd)
        {
            throw new System.NotImplementedException();
        }

        public decimal AvailableFunds { get; private set; }
        public WalletIdentification Identification { get; private set; }
        public List<RecurringChange> RecurringChanges { get; private set; }

        public void CreateChange(RecurringChange recurringChange)
        {
            WithdrawalAdded = recurringChange;
        }

        public RecurringChange WithdrawalAdded { get; private set; }
    }
}