﻿using System;
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
            Assert.That(recurranceRule.AppliesTo(firstDate));
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(1)), Is.False);
        }

        [Test]
        public void Just_today_applies_to_today_only()
        {
            
            var firstDate = DateTime.Today;

            var recurranceRule = new RecurranceSpecificationFactory().Build("OneShot", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.AppliesTo(firstDate));
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(1)), Is.False);
        }

        [Test]
        public void Every_day_is_a_rule_applicable_the_same_day_every_day()
        {
            var firstDate = new DateTime(2014, 07, 19);

            var recurranceRule = new RecurranceSpecificationFactory().Build("DailyRule", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.AppliesTo(firstDate));
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(1)), Is.True);
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(2)), Is.True);
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(3)), Is.True);
        }

        [Test]
        public void Every_week_is_a_rule_applicable_the_same_day_every_week()
        {
            var firstDate = new DateTime(2014, 07, 19);

            var recurranceRule = new RecurranceSpecificationFactory().Build("WeeklyRule", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.AppliesTo(firstDate));
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(6)), Is.False);
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(7)), Is.True);
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(8)), Is.False);
        }

        [Test]
        public void Every_week_does_not_apply_before_first_date()
        {
            var firstDate = new DateTime(2014, 07, 19);

            var recurranceRule = new RecurranceSpecificationFactory().Build("WeeklyRule", firstDate, null);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.AppliesTo(firstDate));
            Assert.That(recurranceRule.AppliesTo(firstDate.AddDays(-7)), Is.False);
        }

        [Test]
        public void Every_week_does_not_apply_after_stop_date_when_it_has_been_applied()
        {
            var startDate = new DateTime(2014, 07, 01);
            var stopDate = startDate.AddDays(14);


            var recurranceRule = new RecurranceSpecificationFactory().Build("WeeklyRule", startDate, stopDate);

            Assert.That(recurranceRule, Is.Not.Null);
            Assert.That(recurranceRule.AppliesTo(startDate));
            Assert.That(recurranceRule.AppliesTo(stopDate));
            Assert.That(recurranceRule.AppliesTo(stopDate.AddDays(7)), Is.False);
        }
    }
}