using System.Collections.Generic;
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
            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>());
            var result = recurringChangeController.Display("foo wallet");

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Display_puts_RecurringChangeListViewModel_on_view()
        {
            const string walletName = "foo wallet";

            var recurringChangeListViewModel = new RecurringChangeListViewModel(new List<string>());
            var recurringChangeListViewModelBuilder = MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>();
            recurringChangeListViewModelBuilder
                .Stub(x => x.Build(walletName))
                .Return(recurringChangeListViewModel);


            var recurringChangeController = new RecurringChangeController(recurringChangeListViewModelBuilder, new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>());
            var viewResult = recurringChangeController.Display(walletName);

            Assert.That(viewResult.Model, Is.EqualTo(recurringChangeListViewModel));
        }

        [Test]
        public void Create_returns_view_with_empty_ViewName_set()
        {
            var withdrawalController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>());

            var walletDay = new WalletDay
                                {
                                    Date = null,
                                    WalletName = null
                                };
            var result = withdrawalController.CreateWithdrawal(walletDay);

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Create_sets_WalletName_on_ViewModel()
        {
            const string walletName = "foo name";
            var withdrawalController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>());

            var walletDay = new WalletDay
            {
                Date = null,
                WalletName = null
            };
            var result = withdrawalController.CreateWithdrawal(walletDay);

            var model = (CreateRecurringChangeViewModel) result.Model;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.WalletName, Is.EqualTo(walletName));
        }

        [Test]
        public void AddNewWithdrawal_returns_last_redirect_set()
        {
            const string expectedController = "bar";
            const string expectedAction = "foo";

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>());
            recurringChangeController.SetRedirect(expectedAction, expectedController, new { walletName = "foobar" });
            var result = recurringChangeController.AddNewWithdrawal("foo name", "foo withdrawal", 123m);

            Assert.That(result.RouteValues["action"], Is.EqualTo(expectedAction));
            Assert.That(result.RouteValues["controller"], Is.EqualTo(expectedController));
        }

        [Test]
        public void AddNewDeposit_returns_last_redirect_set()
        {
            const string expectedController = "bar";
            const string expectedAction = "foo";

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>());
            recurringChangeController.SetRedirect(expectedAction, expectedController, new { walletName = "foobar" });
            var result = recurringChangeController.AddNewDeposit("foo name", "foo withdrawal", 123m);

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

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), addChangeAction);
            recurringChangeController.AddNewDeposit(walletName, changeName, changeAmount);

            var argumentsForFirstCallMadeToAction = addChangeAction.GetArgumentsForCallsMadeOn(x => x.Execute(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<decimal>.Is.Anything, Arg<ICreateRedirects>.Is.Anything))[0];

            Assert.That(argumentsForFirstCallMadeToAction[0], Is.EqualTo(walletName));
            Assert.That(argumentsForFirstCallMadeToAction[1], Is.EqualTo(changeName));
            Assert.That(argumentsForFirstCallMadeToAction[2], Is.EqualTo(changeAmount));
        }

        [Test]
        public void AddNewWithdrawal_hands_change_values_to_action_with_ChangeAmount_inverted()
        {
            const string walletName = "foo name";
            const string changeName = "foo withdrawal";
            const decimal changeAmount = 123m;
            const decimal expectedChangeAmount = 0 - changeAmount;

            var addChangeAction = MockRepository.GenerateMock<IAddRecurringChanges>();

            var recurringChangeController = new RecurringChangeController(MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>(), new CreateRecurringChangeViewModelBuilder(), addChangeAction);
            recurringChangeController.AddNewWithdrawal(walletName, changeName, changeAmount);

            var argumentsForFirstCallMadeToAction = addChangeAction.GetArgumentsForCallsMadeOn(x => x.Execute(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<decimal>.Is.Anything, Arg<ICreateRedirects>.Is.Anything))[0];

            Assert.That(argumentsForFirstCallMadeToAction[0], Is.EqualTo(walletName));
            Assert.That(argumentsForFirstCallMadeToAction[1], Is.EqualTo(changeName));
            Assert.That(argumentsForFirstCallMadeToAction[2], Is.EqualTo(expectedChangeAmount));
        }
    }
}