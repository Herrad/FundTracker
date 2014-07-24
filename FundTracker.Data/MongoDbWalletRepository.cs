using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
        private readonly ICacheThings<WalletIdentification, Wallet> _cache;
        private static string _connectionString;
        private static string _databaseName;

        public MongoDbWalletRepository(IMapMongoWalletsToWallets mongoWalletToWalletMapper, ICacheThings<WalletIdentification, Wallet> cache) : base(new List<Type>{typeof(RecurringChangeCreated), typeof (RecurringChangeModified)})
        {
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _cache = cache;
            _connectionString = ConfigurationManager.AppSettings["MongoConnectionString"];
            _databaseName = ConfigurationManager.AppSettings["DatabaseName"];
        }


        public Wallet Get(WalletIdentification identification)
        {
            var cachedWallet = _cache.Get(identification);
            if (cachedWallet != null)
            {
                return cachedWallet;
            }

            var mongoWallet = GetMongoWallet(identification);
            var mongoRecurringChanges = GetAllRecurringChangesFor(mongoWallet);

            var inflatedWallet = _mongoWalletToWalletMapper.InflateWallet(mongoWallet, mongoRecurringChanges);
            Cache(inflatedWallet);
            return inflatedWallet;
        }

        private void Cache(Wallet wallet)
        {
            _cache.Store(wallet.Identification, wallet);
        }

        private static IEnumerable<MongoRecurringChange> GetAllRecurringChangesFor(MongoWallet mongoWallet)
        {
            var mongoQuery = Query<MongoRecurringChange>.EQ(mw => mw.WalletId, mongoWallet.Id);
            return GetRecurringChanges().Find(mongoQuery);
        }

        public void Save(IHaveChangingFunds wallet)
        {
            CreateNewWallet(wallet);
        }

        public override void Notify(AnEvent anEvent)
        {
            if (anEvent.GetType() == typeof(RecurringChangeCreated))
            {
                var recurringChangeCreated = (RecurringChangeCreated)anEvent;
                var recurringChange = recurringChangeCreated.Change;
                CreateNewRecurringChange(recurringChange, recurringChangeCreated.TargetWallet);
            }
            if (anEvent.GetType() == typeof(RecurringChangeModified))
            {
                var recurringChangeCreated = (RecurringChangeModified)anEvent;
                var recurringChange = recurringChangeCreated.ModifiedChange;
                UpdateExistingRecurringChange(recurringChange, recurringChangeCreated.TargetIdentification);
            }
        }

        private static MongoWallet GetMongoWallet(WalletIdentification identification)
        {
            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var mongoWallet = GetWallets().FindOne(mongoQuery);
            return mongoWallet;
        }

        private static void CreateNewWallet(IAmIdentifiable wallet)
        {
            GetWallets().Insert(new MongoWallet
            {
                Name = wallet.Identification.Name
            });
        }

        private static void CreateNewRecurringChange(RecurringChange recurringChange, WalletIdentification targetWalletIdentifier)
        {
            var wallet = GetMongoWallet(targetWalletIdentifier);

            GetRecurringChanges().Insert(BuildMongoRecurringChange(recurringChange, wallet));
        }

        private void UpdateExistingRecurringChange(RecurringChange recurringChange, WalletIdentification targetIdentification)
        {
            var wallet = GetMongoWallet(targetIdentification);

            var mongoRecurringChangeToModify = GetAllRecurringChangesFor(wallet).First(x => x.Name == recurringChange.Name);
            var mongoQuery = Query<MongoRecurringChange>.EQ(mrc => mrc.Id, mongoRecurringChangeToModify.Id);

            var updatedChange = BuildMongoRecurringChange(recurringChange, wallet);
            var update = Update<MongoRecurringChange>.Set(oldChange => oldChange.LastApplicationDate, updatedChange.LastApplicationDate);

            GetRecurringChanges().Update(mongoQuery, update);
        }

        private static MongoRecurringChange BuildMongoRecurringChange(RecurringChange recurringChange, MongoWallet wallet)
        {
            var lastApplicationDate = recurringChange.EndDate.HasValue ? recurringChange.EndDate.Value.ToString("yyyy-MM-dd") : null;
            return new MongoRecurringChange
            {
                WalletId = wallet.Id,
                Amount = recurringChange.Amount,
                Name = recurringChange.Name,
                FirstApplicationDate = recurringChange.StartDate.ToString("yyyy-MM-dd"),
                LastApplicationDate = lastApplicationDate,
                RecurranceRule = recurringChange.RuleName()
            };
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