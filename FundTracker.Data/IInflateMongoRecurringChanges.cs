using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data
{
    public interface IInflateMongoRecurringChanges
    {
        MongoRecurringChange MapFrom(RecurringChange recurringChange, MongoWallet wallet);
    }
}