using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.Controllers.BoundModels;
using FundTracker.Web.Controllers.ParameterParsers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestRemovingRecurringChanges
    {
        [Test]
        public void Execute_removes_recurring_change_from_wallet()
        {
            const string walletName = "foo wallet";
            var walletIdentification = new WalletIdentification(walletName);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var removeChangeExposer = new RemoveChangeExposer();
            walletService
                .Stub(x => x.FindRecurringChanger(walletIdentification))
                .Return(removeChangeExposer);

            var removeRecurringChangeAction = new RemoveRecurringChangeAction(walletService);

            const string changeName = "withdrawal for foo";
            var walletDay = new WalletDay { Date = "date", WalletName = walletName };

            removeRecurringChangeAction.Execute(walletDay, changeName, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(removeChangeExposer.NameOfLastChangeRemoved, Is.EqualTo(changeName));
        }

        [Test]
        public void SetsRedirect_on_Redirecter()
        {
            const string walletName = "foo wallet";
            var walletIdentification = new WalletIdentification(walletName);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var removeChangeExposer = new RemoveChangeExposer();
            walletService
                .Stub(x => x.FindRecurringChanger(walletIdentification))
                .Return(removeChangeExposer);

            var removeRecurringChangeAction = new RemoveRecurringChangeAction(walletService);

            const string changeName = "withdrawal for foo";
            var walletDay = new WalletDay { Date = "date", WalletName = walletName };

            var redirecter = MockRepository.GenerateStub<ICreateRedirects>();
            removeRecurringChangeAction.Execute(walletDay, changeName, redirecter);

            redirecter.AssertWasCalled(x => x.SetRedirect(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<object>.Is.Anything), c => c.Repeat.Once());
            var argumentsForFirstCallToSetRedirect = redirecter.GetArgumentsForCallsMadeOn(x => x.SetRedirect(Arg<string>.Is.Anything, Arg<string>.Is.Anything, Arg<object>.Is.Anything))[0];

            Assert.That(argumentsForFirstCallToSetRedirect[0], Is.EqualTo("Display"));
            Assert.That(argumentsForFirstCallToSetRedirect[1], Is.EqualTo("RecurringChange"));
        }
    }
}