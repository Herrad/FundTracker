using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class RecurringChangeRemoved : AnEvent
    {
        public WalletIdentification TargetIdentification { get; private set; }
        public RecurringChange ChangeToRemove { get; private set; }

        public RecurringChangeRemoved(WalletIdentification targetIdentification, RecurringChange changeToRemove)
        {
            TargetIdentification = targetIdentification;
            ChangeToRemove = changeToRemove;
        }
    }
}