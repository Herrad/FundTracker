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

            var recurranceRule = new RecurranceSpecificationFactory().Build("foo rule", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(1)), Is.False);
        }

        [Test]
        public void Just_today_applies_to_today_only()
        {
            
            var firstDate = DateTime.Today;

            var recurranceRule = new RecurranceSpecificationFactory().Build("Just today", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(1)), Is.False);
        }

        [Test]
        public void Every_day_is_a_rule_applicable_the_same_day_every_day()
        {
            var firstDate = new DateTime(2014, 07, 19);

            var recurranceRule = new RecurranceSpecificationFactory().Build("Every day", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(1)), Is.True);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(2)), Is.True);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(3)), Is.True);
        }

        [Test]
        public void Every_week_is_a_rule_applicable_the_same_day_every_week()
        {
            var firstDate = new DateTime(2014, 07, 19);

            var recurranceRule = new RecurranceSpecificationFactory().Build("Every week", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(6)), Is.False);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(7)), Is.True);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(8)), Is.False);
        }

        [Test]
        public void Every_week_does_not_apply_before_first_date()
        {
            var firstDate = new DateTime(2014, 07, 19);

            var recurranceRule = new RecurranceSpecificationFactory().Build("Every week", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate));
            Assert.That(recurranceRule.IsSpecifiedOn(firstDate.AddDays(-7)), Is.False);
        }

        [Test]
        public void Every_week_does_not_apply_after_stop_date_when_it_has_been_applied()
        {
            var startDate = new DateTime(2014, 07, 01);
            var stopDate = startDate.AddDays(14);


            var recurranceRule = new RecurranceSpecificationFactory().Build("Every week", startDate, stopDate);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.IsSpecifiedOn(startDate));
            Assert.That(recurranceRule.IsSpecifiedOn(stopDate));
            Assert.That(recurranceRule.IsSpecifiedOn(stopDate.AddDays(7)), Is.False);
        }
    }
}