using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class RecurringChangeModified : AnEvent
    {
        public WalletIdentification TargetIdentification { get; set; }
        public RecurringChange ModifiedChange { get; set; }

        public RecurringChangeModified(WalletIdentification targetIdentification, RecurringChange modifiedChange)
        {
            TargetIdentification = targetIdentification;
            ModifiedChange = modifiedChange;
        }
    }
}