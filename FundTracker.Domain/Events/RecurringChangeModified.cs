using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class RecurringChangeModified : AnEvent
    {
        public WalletIdentification TargetIdentification { get; private set; }
        public RecurringChange ModifiedChange { get; private set; }

        public RecurringChangeModified(WalletIdentification targetIdentification, RecurringChange modifiedChange)
        {
            TargetIdentification = targetIdentification;
            ModifiedChange = modifiedChange;
        }
    }
}