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

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            var addChangeAction = new RemoveRecurringChangeAction(walletService);

            const string changeName = "withdrawal for foo";
            var walletDay = new WalletDay { Date = "date", WalletName = walletName };

            addChangeAction.Execute(walletDay, changeName, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(removeChangeExposer.NameOfLastChangeRemoved, Is.EqualTo(changeName));
        }
    }
}