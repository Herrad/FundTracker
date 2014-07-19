using System;

namespace FundTracker.Domain.RecurranceRules
{
    public class OneShotRule : IDecideWhenRecurringChangesOccur
    {
        private readonly DateTime _dateToApplyTo;

        public OneShotRule(DateTime dateToApplyTo)
        {
            _dateToApplyTo = dateToApplyTo;
        }

        public bool IsSpecifiedOn(DateTime targetDate)
        {
            return _dateToApplyTo == targetDate;
        }

        public string Name { get { return "Just today"; } }
    }
}