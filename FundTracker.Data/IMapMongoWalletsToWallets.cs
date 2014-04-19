using System.Collections.Generic;
using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data
{
    public interface IMapMongoWalletsToWallets
    {
        IWallet InflateWallet(MongoWallet mongoWallet, IEnumerable<MongoRecurringChange> mongoRecurringChanges);
    }
}