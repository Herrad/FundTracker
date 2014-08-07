using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;
using FundTracker.Services;
using FundTracker.Web.Controllers.ParameterParsers;
using FundTracker.Web.ViewModels;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.ViewModels.Builders
{
    [TestFixture]
    public class TestRecurringChangeListViewModelBuilder
    {
        [Test]
        public void Sets_a_list_of_names_and_amounts()
        {
            const string walletName = "foo wallet";
            var startDate = new DateTime(1, 2, 3);

            var expectedChange1 = new RecurringChange(0, "foo1", 1, new OneShotRule(startDate, null));
            var expectedChange2 = new RecurringChange(0, "foo2", 2, new OneShotRule(startDate, null));
            var expectedChange3 = new RecurringChange(0, "foo3", 3, new OneShotRule(startDate, null));

            var recurringChanges = new List<RecurringChange>
            {
                expectedChange1,
                expectedChange2,
                expectedChange3
            };
            var recurringChanger = MockRepository.GenerateStub<IHaveRecurringChanges>();
            recurringChanger
                .Stub(x => x.GetChangesActiveOn(startDate))
                .Return(recurringChanges);

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
            Assert.That(recurringChangeListViewModel.RecurringChangeViewModels, Is.Not.Null);

            var changeNames = recurringChangeListViewModel.RecurringChangeViewModels.ToList();
            Assert.That(changeNames.Count, Is.EqualTo(3));

            Assert.That(changeNames[0].Name, Is.EqualTo(expectedChange1.Name));
            Assert.That(changeNames[0].Amount, Is.EqualTo(expectedChange1.Amount));

            Assert.That(changeNames[1].Name, Is.EqualTo(expectedChange2.Name));
            Assert.That(changeNames[1].Amount, Is.EqualTo(expectedChange2.Amount));

            Assert.That(changeNames[2].Name, Is.EqualTo(expectedChange3.Name));
            Assert.That(changeNames[2].Amount, Is.EqualTo(expectedChange3.Amount));
        }

        [Test]
        public void Only_shows_Changes_where_rule_applies()
        {
            const string walletName = "foo wallet";
            const string walletDate = "foo date";
            
            var startDate = new DateTime(1, 2, 3);
            var expectedChange1 = new RecurringChange(0, "foo1", 0, new OneShotRule(startDate, null));
            var expectedChange2 = new RecurringChange(0, "foo2", 0, new OneShotRule(startDate, null));

            var recurringChanges = new List<RecurringChange>
            {
                expectedChange1,
                expectedChange2
            };
            var recurringChanger = MockRepository.GenerateStub<IHaveRecurringChanges>();
            recurringChanger
                .Stub(x => x.GetChangesActiveOn(startDate))
                .Return(recurringChanges);

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
            Assert.That(recurringChangeListViewModel.RecurringChangeViewModels, Is.Not.Null);
            var changeNames = recurringChangeListViewModel.RecurringChangeViewModels.ToList();
            Assert.That(changeNames.Count, Is.EqualTo(2));
            Assert.That(changeNames[0].Name, Is.EqualTo(expectedChange1.Name));
            Assert.That(changeNames[1].Name, Is.EqualTo(expectedChange2.Name));
        }

        [Test]
        public void Sets_stop_link_text_and_href_for_stopping_change_when_not_a_one_shot_change()
        {
            const string expectedStopLinkText = "Stop from today";
            const string walletName = "foo wallet";
            const string walletDate = "foo date";
            const string changeName = "foo1";

            var startDate = new DateTime(1, 2, 3);
            var selectedDate = new DateTime(1, 1, 1);
            var recurringChange = new RecurringChange(123, changeName, 0, new WeeklyRule(startDate, null));
            var expectedLinkDestination = "/RecurringChange/StopChange/?walletName=" + walletName + "&date=" + selectedDate.ToString("yyyy-MM-dd") + "&changeId=" + 123;

            var recurringChanges = new List<RecurringChange> { recurringChange };
            var recurringChanger = MockRepository.GenerateStub<IHaveRecurringChanges>();
            recurringChanger
                .Stub(x => x.GetChangesActiveOn(startDate))
                .Return(recurringChanges);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(recurringChanger);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(walletDate))
                .Return(selectedDate);

            var recurringChangeListViewModelBuilder = new RecurringChangeListViewModelBuilder(walletService, dateParser);
            var recurringChangeListViewModel = recurringChangeListViewModelBuilder.Build(walletName, walletDate);

            Assert.That(recurringChangeListViewModel, Is.Not.Null);
            Assert.That(recurringChangeListViewModel.RecurringChangeViewModels, Is.Not.Null);
            var changeNames = recurringChangeListViewModel.RecurringChangeViewModels.ToList();

            Assert.That(changeNames[0].StopLinkText, Is.EqualTo(expectedStopLinkText));
            Assert.That(changeNames[0].StopLinkDestination, Is.EqualTo(expectedLinkDestination));
        }

        [Test]
        public void Sets_remove_link_text_and_href_for_stopping_change_when_a_one_shot_change()
        {
            const string expectedStopLinkText = "Remove from today";
            const string walletName = "foo wallet";
            const string walletDate = "foo date";
            const string changeName = "foo1";

            var startDate = new DateTime(1, 2, 3);
            var selectedDate = new DateTime(1, 1, 1);
            var recurringChange = new RecurringChange(123, changeName, 0, new OneShotRule(startDate, startDate));
            var expectedLinkDestination = "/RecurringChange/Delete/?walletName=" + walletName + "&date=" + selectedDate.ToString("yyyy-MM-dd") + "&changeId=" + 123;

            var recurringChanges = new List<RecurringChange> { recurringChange };
            var recurringChanger = MockRepository.GenerateStub<IHaveRecurringChanges>();
            recurringChanger
                .Stub(x => x.GetChangesActiveOn(selectedDate))
                .Return(recurringChanges);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(recurringChanger);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(walletDate))
                .Return(selectedDate);

            var recurringChangeListViewModelBuilder = new RecurringChangeListViewModelBuilder(walletService, dateParser);
            var recurringChangeListViewModel = recurringChangeListViewModelBuilder.Build(walletName, walletDate);

            Assert.That(recurringChangeListViewModel, Is.Not.Null);
            Assert.That(recurringChangeListViewModel.RecurringChangeViewModels, Is.Not.Null);
            var changeNames = recurringChangeListViewModel.RecurringChangeViewModels.ToList();

            Assert.That(changeNames[0].StopLinkText, Is.EqualTo(expectedStopLinkText));
            Assert.That(changeNames[0].StopLinkDestination, Is.EqualTo(expectedLinkDestination));
        }

        [Test]
        public void Sets_remove_link_text_and_href_for_stopping_change_when_start_and_end_dates_are_the_same()
        {
            const string expectedStopLinkText = "Remove from today";
            const string walletName = "foo wallet";
            const string walletDate = "foo date";
            const string changeName = "foo1";

            var startDate = new DateTime(1, 2, 3);
            var selectedDate = new DateTime(1, 1, 1);

            var endDate = startDate;
            var recurringChange = new RecurringChange(123, changeName, 0, new DailyRule(startDate, endDate));
            var expectedLinkDestination = "/RecurringChange/Delete/?walletName=" + walletName + "&date=" + selectedDate.ToString("yyyy-MM-dd") + "&changeId=" + 123;

            var recurringChanges = new List<RecurringChange> { recurringChange };
            var recurringChanger = MockRepository.GenerateStub<IHaveRecurringChanges>();
            recurringChanger
                .Stub(x => x.GetChangesActiveOn(selectedDate))
                .Return(recurringChanges);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(recurringChanger);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(walletDate))
                .Return(selectedDate);

            var recurringChangeListViewModelBuilder = new RecurringChangeListViewModelBuilder(walletService, dateParser);
            var recurringChangeListViewModel = recurringChangeListViewModelBuilder.Build(walletName, walletDate);

            Assert.That(recurringChangeListViewModel, Is.Not.Null);
            Assert.That(recurringChangeListViewModel.RecurringChangeViewModels, Is.Not.Null);
            var changeNames = recurringChangeListViewModel.RecurringChangeViewModels.ToList();

            Assert.That(changeNames[0].StopLinkText, Is.EqualTo(expectedStopLinkText));
            Assert.That(changeNames[0].StopLinkDestination, Is.EqualTo(expectedLinkDestination));
        }

        [Test]
        public void Sets_NavigationLinks()
        {
            const string walletName = "foo name";
            const string walletDate = "foo date";
            var startDate = new DateTime(01, 02, 03);
            var selectedDate = new DateTime(1, 1, 1);

            const string linkText = "Wallet";
            string target = "/Wallet/Display/?walletName=" + walletName + "&date=" + selectedDate;

            var recurringChanges = new List<RecurringChange>();
            var recurringChanger = MockRepository.GenerateStub<IHaveRecurringChanges>();
            recurringChanger
                .Stub(x => x.GetChangesActiveOn(selectedDate))
                .Return(recurringChanges);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(recurringChanger);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(walletDate))
                .Return(selectedDate);

            var recurringChangeListViewModelBuilder = new RecurringChangeListViewModelBuilder(walletService, dateParser);
            var recurringChangeListViewModel = recurringChangeListViewModelBuilder.Build(walletName, walletDate);

            
            var navigationLinkViewModels = recurringChangeListViewModel.NavigationLinkViewModels;
            Assert.That(navigationLinkViewModels, Is.Not.Null);

            var navigationLinkViewModelsList = navigationLinkViewModels.ToList();
            Assert.That(navigationLinkViewModelsList[0].LinkText, Is.EqualTo(linkText));
            Assert.That(navigationLinkViewModelsList[0].Target, Is.EqualTo(target));
        }
    }
}