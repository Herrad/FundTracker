using System;
using System.Collections.Generic;
using System.Configuration;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace Test.Acceptance.FundTracker.Web.Data
{
    public class TestDbAdapter
    {
        private static readonly MongoClient Client = new MongoClient(ConfigurationManager.AppSettings["MongoConnectionString"]);

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

        public static void CreateRecurringChange(string walletName, string nameOfRemoval, int changeAmount, DateTime firstApplicationDate)
        {
            var mongoDatabase = GetMongoDatabase();
            var recurringChanges = mongoDatabase.GetCollection<MongoRecurringChange>("RecurringChanges");
            var wallet = FindWalletCalled(walletName);
            var recurringChangeToInsert = new MongoRecurringChange
            {
                WalletId = wallet.Id,
                Amount = changeAmount,
                Name = nameOfRemoval,
                FirstApplicationDate = firstApplicationDate
            };
            recurringChanges.Insert(recurringChangeToInsert);
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

        public static void RemoveRecurringChangesAssociatedWith(string walletName)
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
            var mongoDatabase = mongoServer.GetDatabase(ConfigurationManager.AppSettings["DatabaseName"]);
            return mongoDatabase;
        }
    }
}