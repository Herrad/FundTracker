using System.Collections.Generic;
using System.Linq;
using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using MicroEvent;

namespace FundTracker.Data.Mappers
{
    [UsedImplicitly]
    public class MongoWalletToWalletMapper : IMapMongoWalletsToWallets
    {
        private readonly IReceivePublishedEvents _eventBus;
        private readonly IMapMongoRecurringChangesToRecurringChanges _mongoRecurringChangeToRecurringChangeMapper;

        public MongoWalletToWalletMapper(IReceivePublishedEvents eventBus, IMapMongoRecurringChangesToRecurringChanges mongoRecurringChangeToRecurringChangeMapper)
        {
            _eventBus = eventBus;
            _mongoRecurringChangeToRecurringChangeMapper = mongoRecurringChangeToRecurringChangeMapper;
        }

        public Wallet InflateWallet(MongoWallet mongoWallet, IEnumerable<MongoRecurringChange> mongoRecurringChanges)
        {
            var walletIdentification = new WalletIdentification(mongoWallet.Name);
            var recurringChanges = mongoRecurringChanges.Select(_mongoRecurringChangeToRecurringChangeMapper.Map).ToList();
            var wallet = new Wallet(_eventBus, walletIdentification, recurringChanges);
            return wallet;
        }
    }
}