using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class RecurringChangeModified : AnEvent
    {
        public WalletIdentification TargetIdentification { get; private set; }
        public RecurringChangeValues RecurringChangeValues { get; private set; }

        public RecurringChangeModified(WalletIdentification targetIdentification, RecurringChangeValues recurringChangeValues)
        {
            RecurringChangeValues = recurringChangeValues;
            TargetIdentification = targetIdentification;
        }
    }
}