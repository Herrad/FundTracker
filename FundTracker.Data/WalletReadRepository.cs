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
        private readonly ICacheThings<WalletIdentification, MongoWallet> _cache;

        public WalletReadRepository(IProvideMongoCollections databaseAdapter, ICacheThings<WalletIdentification, MongoWallet> cache)
        {
            _databaseAdapter = databaseAdapter;
            _cache = cache;
        }

        public MongoWallet GetMongoWallet(WalletIdentification identification)
        {
            if (_cache.EntryExistsFor(identification))
            {
                return _cache.Get(identification);
            }

            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var mongoWallet = _databaseAdapter.GetCollection<MongoWallet>("Wallets").FindOne(mongoQuery);

            _cache.Store(identification, mongoWallet);
            return mongoWallet;
        }
    }
}