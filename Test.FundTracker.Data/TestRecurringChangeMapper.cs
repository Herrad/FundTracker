using System;
using FundTracker.Data.Entities;
using FundTracker.Data.Mappers;
using FundTracker.Domain.RecurranceRules;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Data
{
    [TestFixture]
    public class TestRecurringChangeMapper
    {
        [Test]
        public void Map_sets_Name_and_Amount()
        {
            const string expectedName = "foo change";
            const decimal expectedAmount = 123m;

            var mapper = new MongoRecurringChangeToRecurringChangeMapper(MockRepository.GenerateStub<IBuildRecurranceSpecifications>());
            var recurringChange = mapper.Map(new MongoRecurringChange { Name = expectedName, Amount = expectedAmount, FirstApplicationDate = "2001-02-03"});

            Assert.That(recurringChange.ToValues().Name, Is.EqualTo(expectedName));
            Assert.That(recurringChange.ToValues().Amount, Is.EqualTo(expectedAmount));
        }

        [Test]
        public void Sets_RecurranceSpecification_using_RecurranceSpecificationFactory()
        {
            const string recurranceType = "a recurrance type";

            var targetDate = new DateTime(2001, 02, 03);

            var recurranceSpecification = MockRepository.GenerateStub<IDecideWhenRecurringChangesOccur>();
            recurranceSpecification
                .Stub(x => x.AppliesTo(targetDate))
                .Return(true);

            var recurranceSpecificationFactory = MockRepository.GenerateStub<IBuildRecurranceSpecifications>();
            recurranceSpecificationFactory
                .Stub(x => x.Build(recurranceType, new DateTime(2001, 2, 3), null))
                .Return(recurranceSpecification);

            var mapper = new MongoRecurringChangeToRecurringChangeMapper(recurranceSpecificationFactory);

            var recurringChange = mapper.Map(new MongoRecurringChange {FirstApplicationDate = "2001-02-03", RecurranceType = recurranceType});

            Assert.That(recurringChange.AppliesTo(targetDate));
        }
    }
}
