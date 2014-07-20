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
            var walletDay = new WalletDay {Date = "foo date", WalletName = "foo wallet"};
            var result = recurringChangeController.Display(walletDay);

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Display_puts_RecurringChangeListViewModel_on_view()
        {
            const string walletName = "foo wallet";
            var walletDay = new WalletDay { Date = "foo date", WalletName = walletName };
            var recurringChangeListViewModel = new RecurringChangeListViewModel(new List<RecurringChangeViewModel>());
            var recurringChangeListViewModelBuilder = MockRepository.GenerateStub<IBuildRecurringChangeListViewModels>();
            recurringChangeListViewModelBuilder
                .Stub(x => x.Build(walletName, "foo date"))
                .Return(recurringChangeListViewModel);


            var recurringChangeController = new RecurringChangeController(recurringChangeListViewModelBuilder, new CreateRecurringChangeViewModelBuilder(), MockRepository.GenerateStub<IAddRecurringChanges>());
            var viewResult = recurringChangeController.Display(walletDay);

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
                WalletName = walletName
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
            var result = recurringChangeController.AddNewWithdrawal(new WalletDay {WalletName = "foo name", Date = "foo date"}, new AddedChange {Amount = 123m, ChangeName = "foo withdrawal"});

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
            var result = recurringChangeController.AddNewDeposit(new WalletDay {WalletName = "foo name", Date = "foo date"}, new AddedChange {ChangeName = "foo withdrawal", Amount = 123m});

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
            recurringChangeController.AddNewDeposit(new WalletDay {WalletName = walletName, Date = "foo date"}, new AddedChange {ChangeName = changeName, Amount = changeAmount});

            var argumentsForFirstCallMadeToAction = addChangeAction.GetArgumentsForCallsMadeOn(x => x.Execute(Arg<WalletDay>.Is.Anything, Arg<AddedChange>.Is.Anything, Arg<ICreateRedirects>.Is.Anything))[0];

            Assert.That(((WalletDay)argumentsForFirstCallMadeToAction[0]).WalletName, Is.EqualTo(walletName));
            Assert.That(((AddedChange)argumentsForFirstCallMadeToAction[1]).ChangeName, Is.EqualTo(changeName));
            Assert.That(((AddedChange)argumentsForFirstCallMadeToAction[1]).Amount, Is.EqualTo(changeAmount));
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
            recurringChangeController.AddNewWithdrawal(new WalletDay {WalletName = walletName, Date = "foo date"}, new AddedChange {Amount = changeAmount, ChangeName = changeName});

            var argumentsForFirstCallMadeToAction = addChangeAction.GetArgumentsForCallsMadeOn(x => x.Execute(Arg<WalletDay>.Is.Anything, Arg<AddedChange>.Is.Anything, Arg<ICreateRedirects>.Is.Anything))[0];

            Assert.That(((WalletDay)argumentsForFirstCallMadeToAction[0]).WalletName, Is.EqualTo(walletName));
            Assert.That(((AddedChange)argumentsForFirstCallMadeToAction[1]).ChangeName, Is.EqualTo(changeName));
            Assert.That(((AddedChange)argumentsForFirstCallMadeToAction[1]).Amount, Is.EqualTo(expectedChangeAmount));
        }
    }
}