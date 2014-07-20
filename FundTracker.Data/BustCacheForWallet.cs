using FundTracker.Domain;
using MicroEvent;

namespace FundTracker.Data
{
    public class BustCacheForWallet : AnEvent
    {
        public BustCacheForWallet(WalletIdentification targetWalletIdentification)
        {
            TargetWalletIdentification = targetWalletIdentification;
        }

        public WalletIdentification TargetWalletIdentification { get; private set; }
    }
}