using System.Collections.Generic;
using FundTracker.Data.Entities;
using FundTracker.Domain;
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
            var walletsCollection = GetWalletsCollection();
            var walletToInsert = new MongoWallet {Name = walletName, AvailableFunds = availableFunds};

            if (FindWalletCalled(walletName) != null)
            {
                UpdateExistingWallet(walletToInsert);
                return;
            }

            walletsCollection.Insert(walletToInsert);
        }

        public static void UpdateExistingWallet(MongoWallet walletToUpdate)
        {
            var walletsCollection = GetWalletsCollection();
            var update = Update<MongoWallet>.Set(mw => mw.AvailableFunds, walletToUpdate.AvailableFunds);
            walletsCollection.Update(Query<MongoWallet>.EQ(e => e.Name, walletToUpdate.Name), update);
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