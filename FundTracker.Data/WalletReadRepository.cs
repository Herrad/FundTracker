using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using MongoDB.Driver.Builders;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class WalletReadRepository : IProvideMongoWallets
    {
        private readonly IProvideMongoCollections _databaseAdapter;

        public WalletReadRepository(IProvideMongoCollections databaseAdapter)
        {
            _databaseAdapter = databaseAdapter;
        }

        public MongoWallet GetMongoWallet(WalletIdentification identification)
        {
            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            return _databaseAdapter.GetCollection<MongoWallet>("Wallets").FindOne(mongoQuery);
        }
    }
}