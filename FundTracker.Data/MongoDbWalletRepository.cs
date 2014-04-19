using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly MongoCollection<MongoWallet> _walletCollection = GetMongoDatabase().GetCollection<MongoWallet>("Wallets");
        private readonly MongoCollection<MongoRecurringChange> _mongoRecurringChanges = GetMongoDatabase().GetCollection<MongoRecurringChange>("RecurringChanges");

        public MongoDbWalletRepository(IMapMongoWalletsToWallets mongoWalletToWalletMapper) : base(new List<Type>{typeof(WalletFundsChanged), typeof(RecurringChangeCreated)})
        {
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _walletCollection = GetMongoDatabase().GetCollection<MongoWallet>("Wallets");
        }


        const string ConnectionString = "mongodb://localhost";

        public IWallet Get(WalletIdentification identification)
        {
            var mongoWallet = GetMongoWallet(identification);
            var mongoRecurringChanges = GetAllRecurringChangesFor(mongoWallet);

            return _mongoWalletToWalletMapper.InflateWallet(mongoWallet, mongoRecurringChanges);
        }

        private IEnumerable<MongoRecurringChange> GetAllRecurringChangesFor(MongoWallet mongoWallet)
        {
            var mongoQuery = Query<MongoRecurringChange>.EQ(mw => mw.WalletId, mongoWallet.Id);
            return _mongoRecurringChanges.Find(mongoQuery);
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
                CreateNewRecurringChange(recurringChange);
            }
        }

        private bool WalletExists(IWallet wallet)
        {
            return GetMongoWallet(wallet.Identification) != null;
        }

        private MongoWallet GetMongoWallet(WalletIdentification identification)
        {
            var walletName = identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var mongoWallet = _walletCollection.FindOne(mongoQuery);
            return mongoWallet;
        }

        private void CreateNewWallet(IWallet wallet)
        {
            _walletCollection.Insert(new MongoWallet
            {
                Name = wallet.Identification.Name,
                AvailableFunds = wallet.AvailableFunds
            });
        }

        private void CreateNewRecurringChange(RecurringChange recurringChange)
        {
            var wallet = GetMongoWallet(recurringChange.Identification);

            _mongoRecurringChanges.Insert(new MongoRecurringChange
            {
                WalletId = wallet.Id,
                Amount = recurringChange.Amount
            });
        }

        private void UpdateExistingWallet(IWallet wallet)
        {
            var walletName = wallet.Identification.Name;
            var mongoQuery = Query<MongoWallet>.EQ(mw => mw.Name, walletName);
            var update = Update<MongoWallet>.Set(mw => mw.AvailableFunds, wallet.AvailableFunds);
            _walletCollection.Update(mongoQuery, update);
        }

        private static MongoDatabase GetMongoDatabase()
        {
            var mongoClient = new MongoClient(ConnectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase("FundTracker");
            return mongoDatabase;
        }
    }
}