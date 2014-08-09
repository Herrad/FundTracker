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

        public bool AppliesTo(DateTime targetDate)
        {
            return IsAfterOrOnFirstApplicableDate(targetDate) && IsBeforeOrOnLastApplicableDate(targetDate) && SpecificRulesApplyTo(targetDate);
        }

        private bool IsBeforeOrOnLastApplicableDate(DateTime targetDate)
        {
            return targetDate <= (LastApplicableDate ?? targetDate);
        }

        public DateTime FirstApplicableDate { get; private set; }
        public DateTime? LastApplicableDate { get; private set; }

        public void StopOn(DateTime lastApplicableDate)
        {
            LastApplicableDate = lastApplicableDate;
        }

        public abstract string PrettyPrint();
        protected abstract bool SpecificRulesApplyTo(DateTime targetDate);

        public bool IsOneShot()
        {
            return FirstApplicableDate == LastApplicableDate;
        }
    }
}