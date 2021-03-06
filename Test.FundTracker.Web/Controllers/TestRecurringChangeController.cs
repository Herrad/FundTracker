using System;
using System.Collections.Generic;
using System.Threading;
using FundTracker.Web.Controllers;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.Controllers.BoundModels;
using FundTracker.Web.ViewModels;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestRecurringChangeController
    {
        [Test]
        public void Display_returns_view_with_empty_name()
        {
            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>(), null, null);
            var walletDay = new WalletDay {Date = "foo date", WalletName = "foo wallet"};
            var result = recurringChangeController.Display(walletDay);

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Display_puts_RecurringChangeListViewModel_on_view()
        {
            const string walletName = "foo wallet";
            var walletDay = new WalletDay { Date = "foo date", WalletName = walletName };
            var recurringChangeListViewModel = new RecurringChangeListViewModel(new List<RecurringChangeViewModel>(), new DateTime(1, 2, 3), null);
            var recurringChangeListViewModelBuilder = MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>();
            recurringChangeListViewModelBuilder
                .Stub(x => x.Build(walletName, "foo date"))
                .Return(recurringChangeListViewModel);


            var recurringChangeController = new RecurringChangeController(recurringChangeListViewModelBuilder, new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>(), null, null);
            var viewResult = recurringChangeController.Display(walletDay);

            Assert.That(viewResult.Model, Is.EqualTo(recurringChangeListViewModel));
        }

        [Test]
        public void Create_returns_view_with_Create_ViewName_set()
        {
            var withdrawalController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>(), null, null);

            var walletDay = new WalletDay
                                {
                                    Date = null,
                                    WalletName = null
                                };
            var result = withdrawalController.CreateWithdrawal(walletDay);

            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void Create_sets_WalletName_on_ViewModel()
        {
            const string walletName = "foo name";
            var withdrawalController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>(), null, null);

            var walletDay = new WalletDay
            {
                Date = null,
                WalletName = walletName
            };
            var result = withdrawalController.CreateWithdrawal(walletDay);

            var model = (CreateRecurringChangeViewModel) result.Model;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.WalletName, Is.EqualTo(walletName));
        }

        [Test]
        public void StopChange_redirects_to_last_set_redirect()
        {
            var recurringChangeLimiter = MockRepository.GenerateStub<ILimitRecurringChanges>();
            var recurringChangeController = new RecurringChangeController(null, null, null, recurringChangeLimiter, null);
            recurringChangeController.SetRedirect("foo", "bar", null);
            var result = recurringChangeController.StopChange(new WalletDay(), new IncomingChange());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("foo"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("bar"));
        }

        [Test]
        public void StopChange_passes_WalletDay_and_changeName_to_limiter()
        {
            var walletDay = new WalletDay { WalletName = "foo wallet", Date = "01-02-03" };
            var incomingChange = new IncomingChange { ChangeId = 111 };

            var recurringChangeLimiter = MockRepository.GenerateStub<ILimitRecurringChanges>();
            var recurringChangeController = new RecurringChangeController(null, null, null, recurringChangeLimiter, null);
            recurringChangeController.StopChange(walletDay, incomingChange);

            recurringChangeLimiter
                .AssertWasCalled(x => x.LimitChange(walletDay, incomingChange, recurringChangeController),
                c => c.Repeat.Once());
        }

        [Test]
        public void Delete_passes_WalletDay_and_changeName_to_ChangeRemover()
        {
            var changeRemover = MockRepository.GenerateStub<IRemoveRecurringChanges>();

            var recurringChangeController = new RecurringChangeController(null, null, null, null, changeRemover);
            var walletDay = new WalletDay { WalletName = "foo name", Date = "foo date" };
            var incomingChange = new IncomingChange();
            recurringChangeController.Delete(walletDay, incomingChange);

            changeRemover
                .AssertWasCalled(
                x => x.Execute(walletDay, incomingChange, recurringChangeController),
                c => c.Repeat.Once());
        }

        [Test]
        public void Delete_returns_last_set_redirect()
        {
            var changeRemover = MockRepository.GenerateStub<IRemoveRecurringChanges>();

            var recurringChangeController = new RecurringChangeController(null, null, null, null, changeRemover);
            var walletDay = new WalletDay { WalletName = "foo name", Date = "foo date" };
            recurringChangeController.SetRedirect("foo", "bar", null);
            var result = recurringChangeController.Delete(walletDay, new IncomingChange());

            Assert.That(result, Is.Not.Null);
            Assert.That(result.RouteValues["action"], Is.EqualTo("foo"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("bar"));
        }

        [Test]
        public void AddNewWithdrawal_returns_last_redirect_set()
        {
            const string expectedController = "bar";
            const string expectedAction = "foo";

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>(), null, null);
            recurringChangeController.SetRedirect(expectedAction, expectedController, new { walletName = "foobar" });
            var result = recurringChangeController.AddNewWithdrawal(new WalletDay {WalletName = "foo name", Date = "foo date"}, new IncomingChange {Amount = 123m, ChangeName = "foo withdrawal"});

            Assert.That(result.RouteValues["action"], Is.EqualTo(expectedAction));
            Assert.That(result.RouteValues["controller"], Is.EqualTo(expectedController));
        }

        [Test]
        public void AddNewDeposit_returns_last_redirect_set()
        {
            const string expectedController = "bar";
            const string expectedAction = "foo";

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>(), null, null);
            recurringChangeController.SetRedirect(expectedAction, expectedController, new { walletName = "foobar" });
            var result = recurringChangeController.AddNewDeposit(new WalletDay {WalletName = "foo name", Date = "foo date"}, new IncomingChange {ChangeName = "foo withdrawal", Amount = 123m});

            Assert.That(result.RouteValues["action"], Is.EqualTo(expectedAction));
            Assert.That(result.RouteValues["controller"], Is.EqualTo(expectedController));
        }

        [Test]
        public void AddNewDeposit_hands_change_values_to_action_unchanged()
        {
            const string walletName = "foo name";
            const string changeName = "foo withdrawal";
            const decimal changeAmount = 123m;

            var addChangeAction = MockRepository.GenerateMock<IAddRecurringChanges>();

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), addChangeAction, null, null);
            recurringChangeController.AddNewDeposit(new WalletDay {WalletName = walletName, Date = "foo date"}, new IncomingChange {ChangeName = changeName, Amount = changeAmount});

            var argumentsForFirstCallMadeToAction = addChangeAction.GetArgumentsForCallsMadeOn(x => x.Execute(Arg<WalletDay>.Is.Anything, Arg<IncomingChange>.Is.Anything, Arg<ICreateRedirects>.Is.Anything))[0];

            Assert.That(((WalletDay)argumentsForFirstCallMadeToAction[0]).WalletName, Is.EqualTo(walletName));
            Assert.That(((IncomingChange)argumentsForFirstCallMadeToAction[1]).ChangeName, Is.EqualTo(changeName));
            Assert.That(((IncomingChange)argumentsForFirstCallMadeToAction[1]).Amount, Is.EqualTo(changeAmount));
        }

        [Test]
        public void AddNewWithdrawal_hands_change_values_to_action_with_ChangeAmount_inverted()
        {
            const string walletName = "foo name";
            const string changeName = "foo withdrawal";
            const decimal changeAmount = 123m;
            const decimal expectedChangeAmount = 0 - changeAmount;

            var addChangeAction = MockRepository.GenerateMock<IAddRecurringChanges>();

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), addChangeAction, null, null);
            recurringChangeController.AddNewWithdrawal(new WalletDay {WalletName = walletName, Date = "foo date"}, new IncomingChange {Amount = changeAmount, ChangeName = changeName});

            var argumentsForFirstCallMadeToAction = addChangeAction.GetArgumentsForCallsMadeOn(x => x.Execute(Arg<WalletDay>.Is.Anything, Arg<IncomingChange>.Is.Anything, Arg<ICreateRedirects>.Is.Anything))[0];

            Assert.That(((WalletDay)argumentsForFirstCallMadeToAction[0]).WalletName, Is.EqualTo(walletName));
            Assert.That(((IncomingChange)argumentsForFirstCallMadeToAction[1]).ChangeName, Is.EqualTo(changeName));
            Assert.That(((IncomingChange)argumentsForFirstCallMadeToAction[1]).Amount, Is.EqualTo(expectedChangeAmount));
        }
    }
}