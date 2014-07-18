using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data.Mappers
{
    public interface IMapMongoRecurringChangesToRecurringChanges
    {
        RecurringChange CreateRecurringChange(MongoRecurringChange recurringChange);
    }
}