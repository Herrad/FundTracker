using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class    RecurringChangeCreated : AnEvent
    {
        public RecurringChangeCreated(RecurringChange change, WalletIdentification targetIdentification, RecurringChangeValues recurringChangeValues)
        {
            RecurringChangeValues = recurringChangeValues;
            TargetIdentification = targetIdentification;
        }

        public WalletIdentification TargetIdentification { get; private set; }

        public RecurringChangeValues RecurringChangeValues { get; private set; }
    }
}