using System;
using FundTracker.Domain.RecurranceRules;

namespace FundTracker.Domain
{
    public class RecurringChange
    {
        private readonly IDecideWhenRecurringChangesOccur _recurranceSpecification;

        public RecurringChange(int id, string name, decimal amount, IDecideWhenRecurringChangesOccur recurranceSpecification)
        {
            _recurranceSpecification = recurranceSpecification;
            Id = id;
            Name = name;
            Amount = amount;
        }

        public string Name { get; private set; }
        public decimal Amount { get; private set; }

        public DateTime StartDate { get { return _recurranceSpecification.FirstApplicableDate; } }
        public DateTime? EndDate { get { return _recurranceSpecification.LastApplicableDate; } }
        public int Id { get; private set; }

        public bool AppliesTo(DateTime targetDate)
        {
            return _recurranceSpecification.IsSpecifiedOn(targetDate);
        }

        public string RuleName()
        {
            return _recurranceSpecification.PrettyPrint();
        }

        public void StopOn(DateTime lastApplicableDate)
        {
            _recurranceSpecification.StopOn(lastApplicableDate);
        }

        public string GetRuleType()
        {
            return _recurranceSpecification.GetType().Name;
        }
    }
}