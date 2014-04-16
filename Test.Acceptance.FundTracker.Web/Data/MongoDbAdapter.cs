using System.Collections.Generic;
using FundTracker.Data.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Test.Acceptance.FundTracker.Web.Data
{
    public class MongoDbAdapter
    {
        const string ConnectionString = "mongodb://localhost";
        private static readonly MongoClient Client = new MongoClient(ConnectionString);

        public static void CreateWalletCalled(string walletName, decimal availableFunds)
        {
            if (FindWalletCalled(walletName) != null)
            {
                return;
            }

            var walletsCollection = GetWalletsCollection();

            var walletToInsert = new MongoWallet {Name = walletName, AvailableFunds = availableFunds};
            walletsCollection.Insert(walletToInsert);
        }

        public static MongoWallet FindWalletCalled(string walletName)
        {
            var query = Query<MongoWallet>.EQ(e => e.Name, walletName);
            var walletsCollection = GetWalletsCollection();
            return walletsCollection.FindOne(query);
        }

        private static MongoCollection<MongoWallet> GetWalletsCollection()
        {
            var mongoServer = Client.GetServer();
            var mongoDatabase = mongoServer.GetDatabase("FundTracker");
            return mongoDatabase.GetCollection<MongoWallet>("Wallets");
        }
    }
}