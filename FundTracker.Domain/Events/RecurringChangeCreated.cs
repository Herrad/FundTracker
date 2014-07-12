using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class RecurringChangeCreated : AnEvent
    {
        public RecurringChangeCreated(RecurringChange change, WalletIdentification targetWallet)
        {
            TargetWallet = targetWallet;
            Change = change;
        }

        public RecurringChange Change { get; private set; }
        public WalletIdentification TargetWallet { get; private set; }
    }
}