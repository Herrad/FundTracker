using System.Collections.Generic;
using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data.Mappers
{
    public interface IMapMongoWalletsToWallets
    {
        Wallet InflateWallet(MongoWallet mongoWallet, IEnumerable<MongoRecurringChange> mongoRecurringChanges);
    }
}