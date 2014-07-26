using System;
using System.Collections.Generic;
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
        private readonly IProvideMongoCollections _databaseAdapter;
        private readonly IInflateMongoRecurringChanges _mongoRecurringChangeMapper;
        private readonly IProvideMongoWallets _walletReadRepository;

        public MongoDbWalletRepository(IMapMongoWalletsToWallets mongoWalletToWalletMapper, ICacheThings<WalletIdentification, Wallet> cache, IProvideMongoCollections databaseAdapter, IInflateMongoRecurringChanges mongoRecurringChangeMapper, IProvideMongoWallets walletReadRepository)
            : base(new List<Type> { typeof(RecurringChangeCreated), typeof(RecurringChangeModified), typeof (RecurringChangeRemoved)})
        {
            _mongoWalletToWalletMapper = mongoWalletToWalletMapper;
            _cache = cache;
            _databaseAdapter = databaseAdapter;
            _mongoRecurringChangeMapper = mongoRecurringChangeMapper;
            _walletReadRepository = walletReadRepository;
        }


        public Wallet Get(WalletIdentification identification)
        {
            var cachedWallet = _cache.Get(identification);
            if (cachedWallet != null)
            {
                return cachedWallet;
            }

            var mongoWallet = _walletReadRepository.GetMongoWallet(_databaseAdapter, identification);
            var mongoRecurringChanges = GetAllRecurringChangesFor(mongoWallet);

            var inflatedWallet = _mongoWalletToWalletMapper.InflateWallet(mongoWallet, mongoRecurringChanges);
            Cache(inflatedWallet);
            return inflatedWallet;
        }

        private void Cache(Wallet wallet)
        {
            _cache.Store(wallet.Identification, wallet);
        }

        private IEnumerable<MongoRecurringChange> GetAllRecurringChangesFor(MongoWallet mongoWallet)
        {
            var mongoQuery = Query<MongoRecurringChange>.EQ(mw => mw.WalletId, mongoWallet.Id);
            return GetRecurringChanges().Find(mongoQuery);
        }

        public void Save(IAmIdentifiable wallet)
        {
            _databaseAdapter.GetCollection<MongoWallet>("Wallets").Insert(new MongoWallet
            {
                Name = wallet.Identification.Name
            });
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
            if (anEvent.GetType() == typeof(RecurringChangeRemoved))
            {
                var recurringChangeCreated = (RecurringChangeRemoved)anEvent;
                var recurringChange = recurringChangeCreated.ChangeToRemove;
                DeleteRecurringChange(recurringChange, recurringChangeCreated.TargetIdentification);
            }
        }

        private void CreateNewRecurringChange(RecurringChange recurringChange, WalletIdentification targetWalletIdentifier)
        {
            var wallet = _walletReadRepository.GetMongoWallet(_databaseAdapter, targetWalletIdentifier);

            var mongoRecurringChange = _mongoRecurringChangeMapper.MapFrom(recurringChange, wallet);
            GetRecurringChanges().Insert(mongoRecurringChange);
        }

        private void UpdateExistingRecurringChange(RecurringChange recurringChange, WalletIdentification targetIdentification)
        {
            var wallet = _walletReadRepository.GetMongoWallet(_databaseAdapter, targetIdentification);

            var mongoRecurringChangeToModify = GetAllRecurringChangesFor(wallet).First(x => x.ChangeId == recurringChange.Id);
            var mongoQuery = Query<MongoRecurringChange>.EQ(mrc => mrc.Id, mongoRecurringChangeToModify.Id);

            var updatedChange = _mongoRecurringChangeMapper.MapFrom(recurringChange, wallet);
            var update = Update<MongoRecurringChange>.Set(oldChange => oldChange.LastApplicationDate, updatedChange.LastApplicationDate);

            GetRecurringChanges().Update(mongoQuery, update);
        }
        private void DeleteRecurringChange(RecurringChange recurringChange, WalletIdentification targetIdentification)
        {
            var wallet = _walletReadRepository.GetMongoWallet(_databaseAdapter, targetIdentification);

            var mongoRecurringChangeToModify = GetAllRecurringChangesFor(wallet).First(x => x.ChangeId == recurringChange.Id);
            var mongoQuery = Query<MongoRecurringChange>.EQ(mrc => mrc.Id, mongoRecurringChangeToModify.Id);

            GetRecurringChanges().Remove(mongoQuery);
        }

        private MongoCollection<MongoRecurringChange> GetRecurringChanges()
        {
            return _databaseAdapter.GetCollection<MongoRecurringChange>("RecurringChanges");
        }
    }
}