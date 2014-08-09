using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Domain.Events;
using MongoDB.Bson;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class MongoRecurringChangeMapper : IInflateMongoRecurringChanges
    {
        public MongoRecurringChange MapFrom(ObjectId walletId, RecurringChangeValues recurringChangeValues)
        {
            return new MongoRecurringChange
            {
                WalletId = walletId,
                Amount = recurringChangeValues.Amount,
                Name = recurringChangeValues.Name,
                FirstApplicationDate = recurringChangeValues.StartDate,
                LastApplicationDate = recurringChangeValues.EndDate,
                RecurranceType = recurringChangeValues.RuleType,
                ChangeId = recurringChangeValues.Id
            };
        }
    }
}