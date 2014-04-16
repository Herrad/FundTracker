using System.Collections.Generic;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using FundTracker.Services;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FundTracker.Data
{
    public class MongoDbWalletRepository : ISaveWallets, IKnowAboutWallets
    {
        const string ConnectionString = "mongodb://localhost";

        public void Save(IWallet wallet)
        {
            var walletCollection = GetWalletCollection();

            walletCollection.Insert(new MongoWallet
            {
                Name = wallet.Identification.Name
            });
        }

        public IWallet Get(WalletIdentification identification)
        {
            var walletCollection = GetWalletCollection();

            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var mongoWallet = walletCollection.FindOne(mongoQuery);
            return new Wallet(new WalletIdentification(mongoWallet.Name));
        }

        private static MongoCollection<MongoWallet> GetWalletCollection()
        {
            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase("FundTracker");
            var walletCollection = mongoDatabase.GetCollection<MongoWallet>("Wallets");
            return walletCollection;
        }
    }
}