using FundTracker.Data.Entities;
using FundTracker.Domain;
using MicroEvent;

namespace FundTracker.Data
{
    public class MongoWalletToWalletMapper : IMapMongoWalletsToWallets
    {
        private readonly IReceivePublishedEvents _eventBus;

        public MongoWalletToWalletMapper(IReceivePublishedEvents eventBus)
        {
            _eventBus = eventBus;
        }

        public IWallet InflateWallet(MongoWallet mongoWallet)
        {
            var wallet = new Wallet(new WalletIdentification(mongoWallet.Name), mongoWallet.AvailableFunds, _eventBus);
            return wallet;
        }
    }
}