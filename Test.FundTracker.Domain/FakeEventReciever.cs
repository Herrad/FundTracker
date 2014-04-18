using MicroEvent;

namespace Test.FundTracker.Domain
{
    public class FakeEventReciever : IReceivePublishedEvents
    {
        public AnEvent EventPublished { get; private set; }

        public void Publish(AnEvent anEvent)
        {
            EventPublished = anEvent;
        }
    }
}