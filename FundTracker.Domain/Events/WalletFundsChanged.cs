using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class WalletFundsChanged : AnEvent
    {
        public IAmIdentifiable Wallet { get; private set; }

        public WalletFundsChanged(IAmIdentifiable wallet)
        {
            Wallet = wallet;
        }
    }
}