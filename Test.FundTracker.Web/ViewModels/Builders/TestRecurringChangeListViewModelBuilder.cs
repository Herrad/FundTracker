using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.ParameterParsers;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestRecurringChangeListViewModelBuilder
    {
        [Test]
        public void Sets_a_list_of_names()
        {
            const string walletName = "foo wallet";

            const string expectedName1 = "foo1";
            const string expectedName2 = "foo2";
            const string expectedName3 = "foo3";
            var startDate = new DateTime(1, 2 ,3);
            var recurringChangeNames = new List<string>
            {
                expectedName1,
                expectedName2,
                expectedName3
            };
            var recurringChanger = MockRepository.GenerateStub<IHaveFundsThatFrequentlyChange>();
            recurringChanger
                .Stub(x => x.GetChangeNamesApplicableTo(startDate))
                .Return(recurringChangeNames);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(recurringChanger);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(Arg<string>.Is.Anything))
                .Return(startDate);

            var recurringChangeListViewModelBuilder = new RecurringChangeListViewModelBuilder(walletService, dateParser);
            var recurringChangeListViewModel = recurringChangeListViewModelBuilder.Build(walletName, "foo date");

            Assert.That(recurringChangeListViewModel, Is.Not.Null);
            Assert.That(recurringChangeListViewModel.ChangeNames, Is.Not.Null);

            var changeNames = recurringChangeListViewModel.ChangeNames.ToList();
            Assert.That(changeNames.Count, Is.EqualTo(3));
            Assert.That(changeNames[0], Is.EqualTo(expectedName1));
            Assert.That(changeNames[1], Is.EqualTo(expectedName2));
            Assert.That(changeNames[2], Is.EqualTo(expectedName3));
        }

        [Test]
        public void Only_shows_Changes_where_rule_applies()
        {
            const string walletName = "foo wallet";
            const string walletDate = "foo date";

            const string expectedName1 = "foo1";
            const string expectedName2 = "foo2";
            var startDate = new DateTime(1, 2, 3);

            var recurringChangeNames = new List<string>
            {
                expectedName1,
                expectedName2
            };
            var recurringChanger = MockRepository.GenerateStub<IHaveFundsThatFrequentlyChange>();
            recurringChanger
                .Stub(x => x.GetChangeNamesApplicableTo(startDate))
                .Return(recurringChangeNames);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(recurringChanger);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(walletDate))
                .Return(startDate);

            var recurringChangeListViewModelBuilder = new RecurringChangeListViewModelBuilder(walletService, dateParser);
            var recurringChangeListViewModel = recurringChangeListViewModelBuilder.Build(walletName, walletDate);

            Assert.That(recurringChangeListViewModel, Is.Not.Null);
            Assert.That(recurringChangeListViewModel.ChangeNames, Is.Not.Null);
            var changeNames = recurringChangeListViewModel.ChangeNames.ToList();
            Assert.That(changeNames.Count, Is.EqualTo(2));
            Assert.That(changeNames[0], Is.EqualTo(expectedName1));
            Assert.That(changeNames[1], Is.EqualTo(expectedName2));
        }
    }
}