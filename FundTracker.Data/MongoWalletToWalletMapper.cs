using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data
{
    public class MongoWalletToWalletMapper : IMapMongoWalletsToWallets
    {
        public IWallet InflateWallet(MongoWallet mongoWallet)
        {
            var wallet = new Wallet(new WalletIdentification(mongoWallet.Name));
            wallet.AddFunds(mongoWallet.AvailableFunds);
            return wallet;
        }
    }
}