using System;
using FundTracker.Domain.RecurranceRules;

namespace FundTracker.Domain
{
    public class RecurringChange
    {
        private readonly IDecideWhenRecurringChangesOccur _recurranceSpecification;

        public RecurringChange(string name, decimal amount, DateTime startDate, IDecideWhenRecurringChangesOccur recurranceSpecification)
        {
            _recurranceSpecification = recurranceSpecification;
            StartDate = startDate;
            Name = name;
            Amount = amount;
        }

        public string Name { get; private set; }
        public decimal Amount { get; private set; }

        public DateTime StartDate { get; private set; }

        public bool AppliesTo(DateTime targetDate)
        {
            return _recurranceSpecification.IsSpecifiedOn(targetDate);
        }
    }
}