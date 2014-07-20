using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class WalletFundsChanged : AnEvent
    {
        public IHaveChangingFunds Wallet { get; private set; }

        public WalletFundsChanged(IHaveChangingFunds wallet)
        {
            Wallet = wallet;
        }
    }
}