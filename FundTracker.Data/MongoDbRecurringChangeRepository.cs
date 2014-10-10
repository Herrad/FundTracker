using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using FundTracker.Domain.Events;
using MicroEvent;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class MongoDbRecurringChangeRepository : Subscription, IKnowWhichChangesBelongToWallets
    {
        private readonly IProvideMongoCollections _databaseAdapter;
        private readonly IProvideMongoWallets _walletReadRepository;
        private readonly IInflateMongoRecurringChanges _mongoRecurringChangeMapper;

        public MongoDbRecurringChangeRepository(IProvideMongoCollections databaseAdapter, IProvideMongoWallets walletReadRepository, IInflateMongoRecurringChanges mongoRecurringChangeMapper)
            : base(new List<Type> { typeof(RecurringChangeCreated), typeof(RecurringChangeModified), typeof(RecurringChangeRemoved) })
        {
            _databaseAdapter = databaseAdapter;
            _walletReadRepository = walletReadRepository;
            _mongoRecurringChangeMapper = mongoRecurringChangeMapper;
        }

        private MongoCollection<MongoRecurringChange> GetRecurringChanges()
        {
            return _databaseAdapter.GetCollection<MongoRecurringChange>("RecurringChanges");
        }

        public override void Notify(AnEvent anEvent)
        {
            CreateEvent(anEvent);
            UpdateEvent(anEvent);
            DeleteEvent(anEvent);
        }

        public IEnumerable<MongoRecurringChange> GetAllRecurringChangesFor(MongoWallet mongoWallet)
        {
            var mongoQuery = Query<MongoRecurringChange>.EQ(mw => mw.WalletId, mongoWallet.Id);
            return GetRecurringChanges().Find(mongoQuery);
        }

        private void DeleteEvent(AnEvent anEvent)
        {
            if (anEvent.GetType() == typeof (RecurringChangeRemoved))
            {
                var recurringChangeCreated = (RecurringChangeRemoved) anEvent;
                var recurringChange = recurringChangeCreated.ChangeToRemove;
                DeleteRecurringChange(recurringChange, recurringChangeCreated.TargetIdentification);
            }
        }

        private void DeleteRecurringChange(RecurringChange recurringChange, WalletIdentification targetIdentification)
        {
            var wallet = _walletReadRepository.GetMongoWallet(targetIdentification);

            var mongoRecurringChangeToModify = GetAllRecurringChangesFor(wallet).First(x => x.ChangeId == recurringChange.Id);
            var mongoQuery = Query<MongoRecurringChange>.EQ(mrc => mrc.Id, mongoRecurringChangeToModify.Id);

            GetRecurringChanges().Remove(mongoQuery);
        }

        private void UpdateEvent(AnEvent anEvent)
        {
            if (anEvent.GetType() == typeof (RecurringChangeModified))
            {
                var recurringChangeModified = (RecurringChangeModified)anEvent;
                UpdateExistingRecurringChange(recurringChangeModified.TargetIdentification, recurringChangeModified.RecurringChangeValues);
            }
        }

        private void UpdateExistingRecurringChange(WalletIdentification targetIdentification, RecurringChangeValues recurringChangeValues)
        {
            var wallet = _walletReadRepository.GetMongoWallet(targetIdentification);

            var changeId = recurringChangeValues.Id;
            var mongoRecurringChangeToModify = GetAllRecurringChangesFor(wallet).First(x => x.ChangeId == changeId);
            var mongoQuery = Query<MongoRecurringChange>.EQ(mrc => mrc.Id, mongoRecurringChangeToModify.Id);

            var updatedChange = _mongoRecurringChangeMapper.MapFrom(wallet.Id, recurringChangeValues);
            var update = Update<MongoRecurringChange>
                .Set(oldChange => oldChange.FirstApplicationDate, updatedChange.FirstApplicationDate)
                .Set(oldChange => oldChange.LastApplicationDate, updatedChange.LastApplicationDate)
                .Set(oldChange => oldChange.Name, updatedChange.Name)
                .Set(oldChange => oldChange.RecurranceType, updatedChange.RecurranceType)
                .Set(oldChange => oldChange.Amount, updatedChange.Amount)
                .Set(oldChange => oldChange.ChangeId, updatedChange.ChangeId)
                .Set(oldChange => oldChange.WalletId, updatedChange.WalletId);

            GetRecurringChanges().Update(mongoQuery, update);
        }

        private void CreateEvent(AnEvent anEvent)
        {
            if (anEvent.GetType() == typeof (RecurringChangeCreated))
            {
                var recurringChangeCreated = (RecurringChangeCreated) anEvent;

                CreateNewRecurringChange(recurringChangeCreated.TargetIdentification, recurringChangeCreated.RecurringChangeValues);
            }
        }

        private void CreateNewRecurringChange(WalletIdentification targetWalletIdentifier, RecurringChangeValues recurringChangeValues)
        {
            var wallet = _walletReadRepository.GetMongoWallet(targetWalletIdentifier);
            var mongoRecurringChange = _mongoRecurringChangeMapper.MapFrom(wallet.Id, recurringChangeValues);
            GetRecurringChanges().Insert(mongoRecurringChange);
        }
    }
}