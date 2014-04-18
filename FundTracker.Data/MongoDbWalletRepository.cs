using FundTracker.Data.Entities;
using FundTracker.Domain;
using FundTracker.Domain.Events;
using FundTracker.Services;
using MicroEvent;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FundTracker.Data
{
    public class MongoDbWalletRepository : Subscription, ISaveWallets, IKnowAboutWallets
    {
        private readonly IMapMongoWalletsToWallets _mongoWalletToWalletMapper;
        private readonly MongoCollection<MongoWallet> _walletCollection = GetWalletCollection();

        public MongoDbWalletRepository(IMapMongoWalletsToWallets mongoWalletToWalletMapper) : base(typeof(WalletFundsChanged))
        {
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _walletCollection = GetWalletCollection();
        }


        const string ConnectionString = "mongodb://localhost";

        public IWallet Get(WalletIdentification identification)
        {
            var mongoWallet = GetMongoWallet(identification);

            return _mongoWalletToWalletMapper.InflateWallet(mongoWallet);
        }

        public void Save(IWallet wallet)
        {
            if (WalletExists(wallet))
            {
                UpdateExistingWallet(wallet);
            }

            CreateNewWallet(wallet);
        }

        private bool WalletExists(IWallet wallet)
        {
            return GetMongoWallet(wallet.Identification) != null;
        }

        private void CreateNewWallet(IWallet wallet)
        {
            _walletCollection.Insert(new MongoWallet
            {
                Name = wallet.Identification.Name,
                AvailableFunds = wallet.AvailableFunds
            });
        }

        private void UpdateExistingWallet(IWallet wallet)
        {
            var walletName = wallet.Identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var update = Update<MongoWallet>.Set(mw => mw.AvailableFunds, wallet.AvailableFunds);
            _walletCollection.Update(mongoQuery, update);
        }

        private MongoWallet GetMongoWallet(WalletIdentification identification)
        {
            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var mongoWallet = _walletCollection.FindOne(mongoQuery);
            return mongoWallet;
        }

        private static MongoCollection<MongoWallet> GetWalletCollection()
        {
            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase("FundTracker");
            var walletCollection = mongoDatabase.GetCollection<MongoWallet>("Wallets");
            return walletCollection;
        }

        public override void Notify(AnEvent anEvent)
        {
            var fundsChanged = (WalletFundsChanged) anEvent;
            var wallet = fundsChanged.Wallet;
            UpdateExistingWallet(wallet);
        }
    }
}