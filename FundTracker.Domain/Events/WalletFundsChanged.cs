using MicroEvent;

namespace FundTracker.Domain.Events
{
    public class WalletFundsChanged : AnEvent
    {
        public IWallet Wallet { get; private set; }

        public WalletFundsChanged(IWallet wallet)
        {
            Wallet = wallet;
        }
    }
}