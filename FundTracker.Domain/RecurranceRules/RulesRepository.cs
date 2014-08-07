using System;
using System.Collections.Generic;

namespace FundTracker.Domain.RecurranceRules
{
    public class RulesRepository
    {
        public IEnumerable<IDecideWhenRecurringChangesOccur> GetAvailableRules(DateTime firstApplicableDate, DateTime? lastApplicableDate)
        {
            return new List<IDecideWhenRecurringChangesOccur>
            {
                new WeeklyRule(firstApplicableDate, lastApplicableDate),
                new DailyRule(firstApplicableDate, lastApplicableDate),
                new OneShotRule(firstApplicableDate, lastApplicableDate)
            };
        }
    }
}