using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class    RecurringChangeCreated : AnEvent
    {
        public RecurringChangeCreated(RecurringChange change, WalletIdentification targetIdentification)
        {
            TargetIdentification = targetIdentification;
            Change = change;
        }

        public RecurringChange Change { get; private set; }
        public WalletIdentification TargetIdentification { get; private set; }
    }
}