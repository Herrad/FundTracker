using System;
using FundTracker.Data.Entities;
using FundTracker.Domain;

namespace FundTracker.Data.Mappers
{
    public class MongoRecurringChangeToRecurringChangeMapper : IMapMongoRecurringChangesToRecurringChanges
    {
        public RecurringChange CreateRecurringChange(MongoRecurringChange recurringChange)
        {
            return new RecurringChange(recurringChange.Name, recurringChange.Amount, DateTime.Parse(recurringChange.FirstApplicationDate));
        }
    }
}