using System;
using System.Collections.Generic;
using FundTracker.Domain;

namespace Test.FundTracker.Web.Controllers
{
    public class WithdrawalExposer : IWallet
    {
        void ITakeFundsToAdd.AddFunds(decimal fundsToAdd)
        {
            throw new NotImplementedException();
        }

        decimal IHaveAvailableFunds.AvailableFunds { get { return 0; } }
        WalletIdentification IWallet.Identification { get { return null; } }

        public void CreateChange(RecurringChange recurringChange)
        {
            WithdrawalAdded = recurringChange;
        }

        IEnumerable<RecurringChange> IHaveRecurringChanges.GetRecurringDeposits()
        {
            throw new NotImplementedException();
        }

        IEnumerable<RecurringChange> IHaveRecurringChanges.GetRecurringWithdrawals()
        {
            throw new NotImplementedException();
        }

        IEnumerable<string> IHaveRecurringChanges.GetChangeNamesApplicableTo(DateTime selectedDate)
        {
            throw new NotImplementedException();
        }

        public RecurringChange WithdrawalAdded { get; private set; }
    }
}