using System;
using System.Linq;

namespace FundTracker.Domain.RecurranceRules
{
    public class RecurranceSpecificationFactory : IBuildRecurranceSpecifications
    {
        private readonly RulesRepository _rulesRepository;

        public RecurranceSpecificationFactory()
        {
            _rulesRepository = new RulesRepository();
        }

        public IDecideWhenRecurringChangesOccur Build(string aRecurranceRule, DateTime firstApplicableDate, DateTime? lastApplicableDate)
        {
            var availableRules = _rulesRepository.GetAvailableRules(firstApplicableDate, lastApplicableDate);
            var selectedRule = availableRules.FirstOrDefault(rule => rule.Matches(aRecurranceRule));

            return selectedRule ?? new OneShotRule(firstApplicableDate, lastApplicableDate);
        }
    }
}