using System;

namespace FundTracker.Domain.RecurranceRules
{
    public abstract class RecurranceRule : IDecideWhenRecurringChangesOccur
    {
        protected RecurranceRule(DateTime firstApplicableDate, DateTime? lastApplicableDate)
        {
            FirstApplicableDate = firstApplicableDate;
            LastApplicableDate = lastApplicableDate;
        }
        private bool IsAfterOrOnFirstApplicableDate(DateTime targetDate)
        {
            return targetDate >= FirstApplicableDate;
        }

        public bool IsSpecifiedOn(DateTime targetDate)
        {
            return IsAfterOrOnFirstApplicableDate(targetDate) && IsBeforeOrOnLastApplicableDate(targetDate) && SpecificRulesApplyTo(targetDate);
        }

        private bool IsBeforeOrOnLastApplicableDate(DateTime targetDate)
        {
            return targetDate <= (LastApplicableDate ?? targetDate);
        }

        protected abstract bool SpecificRulesApplyTo(DateTime targetDate);

        public abstract string Name { get; }

        public DateTime FirstApplicableDate { get; private set; }
        public DateTime? LastApplicableDate { get; private set; }

        public void StopOn(DateTime lastApplicableDate)
        {
            LastApplicableDate = lastApplicableDate;
        }

        public bool ApplicableFor(string ruleName)
        {
            return Name == ruleName;
        }
    }
}