using System.Collections.Generic;
using FundTracker.Data.Entities;

namespace FundTracker.Data
{
    public interface IKnowWhichChangesBelongToWallets
    {
        IEnumerable<MongoRecurringChange> GetAllRecurringChangesFor(MongoWallet mongoWallet);
    }
}