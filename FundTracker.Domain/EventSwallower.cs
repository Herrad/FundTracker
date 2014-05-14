using MicroEvent;

namespace FundTracker.Domain
{
    public class EventSwallower : IReceivePublishedEvents
    {
        public void Publish(AnEvent anEvent)
        {
            
        }
    }
}