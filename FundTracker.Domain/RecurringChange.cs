using System;
using FundTracker.Domain.RecurranceRules;

namespace FundTracker.Domain
{
    public class RecurringChange
    {
        private readonly IDecideWhenRecurringChangesOccur _recurranceSpecification;

        public RecurringChange(string name, decimal amount, IDecideWhenRecurringChangesOccur recurranceSpecification)
        {
            _recurranceSpecification = recurranceSpecification;
            Name = name;
            Amount = amount;
        }

        public string Name { get; private set; }
        public decimal Amount { get; private set; }

        public DateTime StartDate { get { return _recurranceSpecification.FirstApplicableDate; } }

        public bool AppliesTo(DateTime targetDate)
        {
            return _recurranceSpecification.IsSpecifiedOn(targetDate);
        }

        public string RuleName()
        {
            return _recurranceSpecification.Name;
        }

        public void StopOn(DateTime lastApplicableDate)
        {
            _recurranceSpecification.StopOn(lastApplicableDate);
        }
    }
}