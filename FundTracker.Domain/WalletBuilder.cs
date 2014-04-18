using MicroEvent;

namespace FundTracker.Domain
{
    public class WalletBuilder : ICreateWallets
    {
        private readonly IStoreCreatedWalets _walletStore;
        private readonly IReceivePublishedEvents _eventBus;

        public WalletBuilder(IStoreCreatedWalets walletStore, IReceivePublishedEvents eventBus)
        {
            _walletStore = walletStore;
            _eventBus = eventBus;
        }

        public void CreateWallet(WalletIdentification walletIdentification)
        {
            _walletStore.Add(new Wallet(walletIdentification, 0, _eventBus));
        }
    }
}