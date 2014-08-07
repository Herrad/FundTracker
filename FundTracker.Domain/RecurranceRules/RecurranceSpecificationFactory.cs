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

        public IDecideWhenRecurringChangesOccur Build(string ruleType, DateTime firstApplicableDate, DateTime? lastApplicableDate)
        {
            var availableRules = _rulesRepository.GetAvailableRules(firstApplicableDate, lastApplicableDate).ToList();
            var selectedRule = availableRules.FirstOrDefault(rule => rule.GetType().Name == ruleType);

            return selectedRule ?? new OneShotRule(firstApplicableDate, firstApplicableDate);
        }
    }
}