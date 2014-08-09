using FundTracker.Data.Entities;
using FundTracker.Domain.Events;
using MongoDB.Bson;

namespace FundTracker.Data
{
    public interface IInflateMongoRecurringChanges
    {
        MongoRecurringChange MapFrom(ObjectId walletId, RecurringChangeValues recurringChangeValues);
    }
}