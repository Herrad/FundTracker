using System;
using System.Collections.Generic;
using System.Configuration;
using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Data.Mappers;
using FundTracker.Domain;
using FundTracker.Domain.Events;
using FundTracker.Services;
using MicroEvent;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class MongoDbWalletRepository : Subscription, ISaveWallets, IKnowAboutWallets
    {
        private readonly IMapMongoWalletsToWallets _mongoWalletToWalletMapper;
        private static string _connectionString;
        private static string _databaseName;

        public MongoDbWalletRepository(IMapMongoWalletsToWallets mongoWalletToWalletMapper) : base(new List<Type>{typeof(WalletFundsChanged), typeof(RecurringChangeCreated)})
        {
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _connectionString = ConfigurationManager.AppSettings["MongoConnectionString"];
            _databaseName = ConfigurationManager.AppSettings["DatabaseName"];
        }


        public IWallet Get(WalletIdentification identification)
        {
            var mongoWallet = GetMongoWallet(identification);
            var mongoRecurringChanges = GetAllRecurringChangesFor(mongoWallet);

            return _mongoWalletToWalletMapper.InflateWallet(mongoWallet, mongoRecurringChanges);
        }

        private static IEnumerable<MongoRecurringChange> GetAllRecurringChangesFor(MongoWallet mongoWallet)
        {
            var mongoQuery = Query<MongoRecurringChange>.EQ(mw => mw.WalletId, mongoWallet.Id);
            return GetRecurringChanges().Find(mongoQuery);
        }

        public void Save(IWallet wallet)
        {
            if (WalletExists(wallet))
            {
                UpdateExistingWallet(wallet);
            }

            CreateNewWallet(wallet);
        }

        public override void Notify(AnEvent anEvent)
        {
            if(anEvent.GetType() == typeof(WalletFundsChanged))
            {
                var fundsChanged = (WalletFundsChanged) anEvent;
                var wallet = fundsChanged.Wallet;
                UpdateExistingWallet(wallet);
            }
            else if (anEvent.GetType() == typeof(RecurringChangeCreated))
            {
                var recurringChangeCreated = (RecurringChangeCreated)anEvent;
                var recurringChange = recurringChangeCreated.Change;
                CreateNewRecurringChange(recurringChange, recurringChangeCreated.TargetWallet);
            }
        }

        private static bool WalletExists(IAmIdentifiable wallet)
        {
            return GetMongoWallet(wallet.Identification) != null;
        }

        private static MongoWallet GetMongoWallet(WalletIdentification identification)
        {
            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var mongoWallet = GetWallets().FindOne(mongoQuery);
            return mongoWallet;
        }

        private static void CreateNewWallet(IHaveChangingFunds wallet)
        {
            GetWallets().Insert(new MongoWallet
            {
                Name = wallet.Identification.Name,
                AvailableFunds = wallet.AvailableFunds
            });
        }

        private static void CreateNewRecurringChange(RecurringChange recurringChange, WalletIdentification targetWalletIdentifier)
        {
            var wallet = GetMongoWallet(targetWalletIdentifier);

            GetRecurringChanges().Insert(new MongoRecurringChange
            {
                WalletId = wallet.Id,
                Amount = recurringChange.Amount,
                Name = recurringChange.Name,
                FirstApplicationDate = recurringChange.StartDate.ToString("yyyy-MM-dd"),
                RecurranceRule = recurringChange.RuleName()
            });
        }

        private static void UpdateExistingWallet(IHaveChangingFunds wallet)
        {
            var walletName = wallet.Identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var update = Update<MongoWallet>.Set(mw => mw.AvailableFunds, wallet.AvailableFunds);
            GetWallets().Update(mongoQuery, update);
        }

        private static MongoCollection<MongoWallet> GetWallets()
        {
            return GetMongoDatabase().GetCollection<MongoWallet>("Wallets");
        }

        private static MongoCollection<MongoRecurringChange> GetRecurringChanges()
        {
            return GetMongoDatabase().GetCollection<MongoRecurringChange>("RecurringChanges");
        }

        private static MongoDatabase GetMongoDatabase()
        {
            var mongoClient = new MongoClient(_connectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(_databaseName);
            return mongoDatabase;
        }
    }
}