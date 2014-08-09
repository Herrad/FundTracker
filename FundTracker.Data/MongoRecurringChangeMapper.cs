using FundTracker.Data.Annotations;
using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data
{
    [UsedImplicitly]
    public class MongoRecurringChangeMapper : IInflateMongoRecurringChanges
    {
        public MongoRecurringChange MapFrom(RecurringChange recurringChange, MongoWallet wallet)
        {
            var lastApplicationDate = recurringChange.EndDate.HasValue ? recurringChange.EndDate.Value.ToString("yyyy-MM-dd") : null;
            return new MongoRecurringChange
            {
                WalletId = wallet.Id,
                Amount = recurringChange.Amount,
                Name = recurringChange.Name,
                FirstApplicationDate = recurringChange.StartDate.ToString("yyyy-MM-dd"),
                LastApplicationDate = lastApplicationDate,
                RecurranceType = recurringChange.GetRuleType(),
                ChangeId = recurringChange.Id
            };
        }
    }
}