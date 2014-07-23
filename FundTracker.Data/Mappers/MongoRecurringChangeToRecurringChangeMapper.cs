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
            var recurranceSpecification = _recurranceSpecificationFactory.Build(recurringChange.RecurranceRule, startDate, null);

            return new RecurringChange(recurringChange.Name, recurringChange.Amount, startDate, recurranceSpecification);
        }
    }
}