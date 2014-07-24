using System;
using FundTracker.Data.Entities;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;

namespace FundTracker.Data.Mappers
{
    public class MongoRecurringChangeToRecurringChangeMapper : IMapMongoRecurringChangesToRecurringChanges
    {
        private readonly IBuildRecurranceSpecifications _recurranceSpecificationFactory;

        public MongoRecurringChangeToRecurringChangeMapper(IBuildRecurranceSpecifications recurranceSpecificationFactory)
        {
            _recurranceSpecificationFactory = recurranceSpecificationFactory;
        }

        public RecurringChange Map(MongoRecurringChange recurringChange)
        {
            var startDate = DateTime.Parse(recurringChange.FirstApplicationDate);
            var endDate = GetEndDate(recurringChange);

            var recurranceSpecification = _recurranceSpecificationFactory.Build(recurringChange.RecurranceRule, startDate, endDate);

            return new RecurringChange(recurringChange.ChangeId, recurringChange.Name, recurringChange.Amount, recurranceSpecification);
        }

        private static DateTime? GetEndDate(MongoRecurringChange recurringChange)
        {
            return recurringChange.LastApplicationDate !=null ? DateTime.Parse(recurringChange.LastApplicationDate) : (DateTime?) null;
        }
    }
}