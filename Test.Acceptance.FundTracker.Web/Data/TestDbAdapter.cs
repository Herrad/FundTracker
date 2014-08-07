using System.Configuration;
using FundTracker.Data.Entities;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Test.Acceptance.FundTracker.Web.Data
{
    public static class TestDbAdapter
    {
        private static readonly MongoClient Client = GetDefaultMongoClient();

        private static MongoClient GetDefaultMongoClient()
        {
            var connectionString = ConfigurationManager.AppSettings["MongoConnectionString"] ?? "mongodb://developer:creat10n@ds053109.mongolab.com:53109/appharbor_e752f405-e12c-4b27-ac06-25d741636009";
            return new MongoClient(connectionString);
        }

        public static void CreateWalletCalled(string walletName)
        {
            var wallet = FindWalletCalled(walletName);
            var walletsCollection = GetWalletsCollection();
            if (wallet != null)
            {
                RemoveAllRecurringChangesAssociatedWith(walletName);
                walletsCollection.Remove(Query<MongoWallet>.EQ(mw => mw.Id, wallet.Id));
            }
            var walletToInsert = new MongoWallet {Name = walletName};

            walletsCollection.Insert(walletToInsert);
        }

        public static void CreateRecurringChange(string walletName, int changeId, string nameOfRemoval, int changeAmount, string firstApplicationDate, string recurranceType)
        {
            var mongoDatabase = GetMongoDatabase();
            var recurringChanges = mongoDatabase.GetCollection<MongoRecurringChange>("RecurringChanges");
            var wallet = FindWalletCalled(walletName);
            var recurringChangeToInsert = new MongoRecurringChange
            {
                WalletId = wallet.Id,
                ChangeId = changeId,
                Amount = changeAmount,
                Name = nameOfRemoval,
                FirstApplicationDate = firstApplicationDate,
                RecurranceType = recurranceType
            };
            recurringChanges.Insert(recurringChangeToInsert);
        }

        public static MongoWallet FindWalletCalled(string walletName)
        {
            var query = Query<MongoWallet>.EQ(e => e.Name, walletName);
            var walletsCollection = GetWalletsCollection();
            return walletsCollection.FindOne(query);
        }

        public static void RemoveAllRecurringChangesAssociatedWith(string walletName)
        {
            var mongoWallet = FindWalletCalled(walletName);
            var mongoDatabase = GetMongoDatabase();
            var recurringChanges = mongoDatabase.GetCollection<MongoRecurringChange>("RecurringChanges");

            var allChangesForWalletQuery = Query<MongoRecurringChange>.EQ(e => e.WalletId, mongoWallet.Id);

            recurringChanges.Remove(allChangesForWalletQuery);
        }

        public static void RemoveWallet(string walletName)
        {
            var query = Query<MongoWallet>.EQ(e => e.Name, walletName);
            GetWalletsCollection().Remove(query);
        }

        private static MongoCollection<MongoWallet> GetWalletsCollection()
        {
            var mongoDatabase = GetMongoDatabase();
            return mongoDatabase.GetCollection<MongoWallet>("Wallets");
        }

        private static MongoDatabase GetMongoDatabase()
        {
            var mongoServer = Client.GetServer();
            var databaseName = ConfigurationManager.AppSettings["DatabaseName"] ?? "appharbor_e752f405-e12c-4b27-ac06-25d741636009";
            var mongoDatabase = mongoServer.GetDatabase(databaseName);
            return mongoDatabase;
        }
    }
}