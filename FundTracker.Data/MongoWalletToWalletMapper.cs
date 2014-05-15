using System.Collections.Generic;
using System.Linq;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using MicroEvent;

namespace FundTracker.Data
{
    public class MongoWalletToWalletMapper : IMapMongoWalletsToWallets
    {
        private readonly IReceivePublishedEvents _eventBus;

        public MongoWalletToWalletMapper(IReceivePublishedEvents eventBus)
        {
            _eventBus = eventBus;
        }

        public IWallet InflateWallet(MongoWallet mongoWallet, IEnumerable<MongoRecurringChange> mongoRecurringChanges)
        {
            var walletIdentification = new WalletIdentification(mongoWallet.Name);
            var recurringChanges = mongoRecurringChanges.Select(x => CreateRecurringChange(x, walletIdentification)).ToList();
            var wallet = new Wallet(_eventBus, walletIdentification, mongoWallet.AvailableFunds, recurringChanges);
            return wallet;
        }

        private static RecurringChange CreateRecurringChange(MongoRecurringChange recurringChange, WalletIdentification walletIdentification)
        {
            return new RecurringChange(walletIdentification, recurringChange.Amount);
        }
    }
}