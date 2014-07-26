using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using MongoDB.Driver.Builders;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class WalletReadRepository : IProvideMongoWallets
    {
        public MongoWallet GetMongoWallet(IProvideMongoCollections databaseAdapter, WalletIdentification identification)
        {
            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var mongoWallet = databaseAdapter.GetCollection<MongoWallet>("Wallets").FindOne(mongoQuery);
            return mongoWallet;
        }
    }
}