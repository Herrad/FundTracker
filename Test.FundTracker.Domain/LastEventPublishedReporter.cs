using MicroEvent;

namespace Test.FundTracker.Domain
{
    public class LastEventPublishedReporter : IReceivePublishedEvents
    {
        public AnEvent LastEventPublished { get; private set; }

        public void Publish(AnEvent anEvent)
        {
            LastEventPublished = anEvent;
        }
    }
}