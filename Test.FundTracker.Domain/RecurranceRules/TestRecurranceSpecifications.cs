using System;
using FundTracker.Domain.RecurranceRules;
using NUnit.Framework;

namespace Test.FundTracker.Domain.RecurranceRules
{
    [TestFixture]
    public class TestRecurranceSpecifications
    {
        [Test]
        public void Defaults_to_one_shot_rule_applicable_only_today()
        {
            var firstDate = DateTime.Today;

            var recurranceRule = new RecurranceSpecificationFactory().Build("foo rule", firstDate);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(1)), Is.False);
        }

        [Test]
        public void Just_today_applies_to_today_only()
        {
            
            var firstDate = DateTime.Today;

            var recurranceRule = new RecurranceSpecificationFactory().Build("Just today", firstDate);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(1)), Is.False);
        }

        [Test]
        public void Every_week_is_a_rule_applicable_the_same_day_every_week()
        {
            var firstDate = new DateTime(2014, 07, 19);

            var recurranceRule = new RecurranceSpecificationFactory().Build("Every week", firstDate);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(6)), Is.False);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(7)), Is.True);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(8)), Is.False);
        }
    }
}