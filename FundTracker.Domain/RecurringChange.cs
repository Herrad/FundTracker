using System;
using FundTracker.Domain.Events;
using FundTracker.Domain.RecurranceRules;

namespace FundTracker.Domain
{
    public class RecurringChange
    {
        private readonly IDecideWhenRecurringChangesOccur _recurranceSpecification;
        private readonly string _name;
        private readonly decimal _amount;

        public RecurringChange(int id, string name, decimal amount, IDecideWhenRecurringChangesOccur recurranceSpecification)
        {
            _recurranceSpecification = recurranceSpecification;
            Id = id;
            _name = name;
            _amount = amount;
        }

        public decimal Amount
        {
            get { return _amount; }
        }

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
            return new RecurringChangeValues(_amount, _name, GetFormattedStartDate(), GetFormattedEndDate(), GetRuleType(), Id);
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