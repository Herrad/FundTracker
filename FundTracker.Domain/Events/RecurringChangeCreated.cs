using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class RecurringChangeCreated : AnEvent
    {
        public RecurringChangeCreated(RecurringChange change)
        {
            Change = change;
        }

        public RecurringChange Change { get; private set; }
    }
}