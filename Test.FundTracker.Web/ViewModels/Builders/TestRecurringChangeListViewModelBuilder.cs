using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;
using FundTracker.Services;
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
        public void Sets_a_list_of_names()
        {
            const string walletName = "foo wallet";

            const string expectedName1 = "foo1";
            const string expectedName2 = "foo2";
            const string expectedName3 = "foo3";
            var recurringChanges = new List<RecurringChange>
            {
                new RecurringChange(expectedName1, 111m),
                new RecurringChange(expectedName2, 222m),
                new RecurringChange(expectedName3, 333m)
            };
            var recurringChanger = MockRepository.GenerateStub<IWallet>();
            recurringChanger
                .Stub(x => x.RecurringChanges)
                .Return(recurringChanges);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindFirstWalletWith(new WalletIdentification(walletName)))
                .Return(recurringChanger);

            var recurringChangeListViewModelBuilder = new RecurringChangeListViewModelBuilder(walletService);
            var recurringChangeListViewModel = recurringChangeListViewModelBuilder.Build(walletName);

            Assert.That(recurringChangeListViewModel, Is.Not.Null);
            Assert.That(recurringChangeListViewModel.ChangeNames, Is.Not.Null);
            var changeNames = recurringChangeListViewModel.ChangeNames.ToList();
            Assert.That(changeNames[0], Is.EqualTo(expectedName1));
            Assert.That(changeNames[1], Is.EqualTo(expectedName2));
            Assert.That(changeNames[2], Is.EqualTo(expectedName3));
        }
    }
}