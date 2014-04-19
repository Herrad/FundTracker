using System.Collections.Generic;
using MicroEvent;

namespace FundTracker.Domain
{
    public class WalletBuilder : ICreateWallets
    {
        private readonly IStoreCreatedWallets _walletStore;
        private readonly IReceivePublishedEvents _eventBus;

        public WalletBuilder(IStoreCreatedWallets walletStore, IReceivePublishedEvents eventBus)
        {
            _walletStore = walletStore;
            _eventBus = eventBus;
        }

        public void CreateWallet(WalletIdentification walletIdentification)
        {
            var wallet = new Wallet(_eventBus, walletIdentification, 0, new List<RecurringChange>());
            _walletStore.Add(wallet);
        }
    }
}