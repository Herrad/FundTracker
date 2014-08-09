using System;
using FundTracker.Domain.Events;
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

        public decimal Amount { get; private set; }
        public string Name { get; private set; }

        public int Id { get; private set; }

        public bool AppliesTo(DateTime targetDate)
        {
            return _recurranceSpecification.AppliesTo(targetDate);
        }

        public string RuleDescription()
        {
            return _recurranceSpecification.PrettyPrint();
        }

        public void StopOn(DateTime lastApplicableDate)
        {
            _recurranceSpecification.StopOn(lastApplicableDate);
        }

        private string GetRuleType()
        {
            return _recurranceSpecification.GetType().Name;
        }

        public string GetStopChangeText()
        {
            return _recurranceSpecification.IsOneShot() ? "Remove from today" : "Stop from today";
        }

        public bool CanBeDeleted()
        {
            return _recurranceSpecification.IsOneShot();
        }

        public RecurringChangeValues ToValues()
        {
            return new RecurringChangeValues(Amount, Name, GetFormattedStartDate(), GetFormattedEndDate(), GetRuleType(), Id);
        }

        private string GetFormattedStartDate()
        {
            return _recurranceSpecification.FirstApplicableDate.ToString("yyyy-MM-dd");
        }

        private string GetFormattedEndDate()
        {
            var lastApplicableDate = _recurranceSpecification.LastApplicableDate;
            return lastApplicableDate.HasValue ? lastApplicableDate.Value.ToString("yyyy-MM-dd") : null;
        }
    }
}