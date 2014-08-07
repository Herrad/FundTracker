using System;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;
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
    public class TestAddRecurringChanges
    {
        [Test]
        public void Execute_puts_new_change_on_wallet_with_identification_and_amount_set()
        {
            const string walletName = "foo wallet";
            var walletIdentification = new WalletIdentification(walletName);
            const decimal expectedAmountGivenToWallet = 123;

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new AddChangeExposer();
            walletService
                .Stub(x => x.FindRecurringChanger(walletIdentification))
                .Return(withdrawalExposer);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            var addChangeAction = new AddChangeAction(walletService, dateParser, MockRepository.GenerateStub<IBuildRecurranceSpecifications>());

            const string withdrawalName = "withdrawal for foo";
            var addedChange = new IncomingChange{Amount = 123,ChangeName = withdrawalName};
            var walletDay = new WalletDay {Date = "date", WalletName = walletName};

            addChangeAction.Execute(walletDay, addedChange, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(withdrawalExposer.LastChangeAdded, Is.Not.Null);
            Assert.That(withdrawalExposer.LastChangeAdded.Amount, Is.EqualTo(expectedAmountGivenToWallet));
            Assert.That(withdrawalExposer.LastChangeAdded.Name, Is.EqualTo(withdrawalName));
        }

        [Test]
        public void Execute_puts_parsed_date_on_wallet()
        {
            const string walletName = "foo wallet";
            var walletIdentification = new WalletIdentification(walletName);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new AddChangeExposer();
            walletService
                .Stub(x => x.FindRecurringChanger(walletIdentification))
                .Return(withdrawalExposer);

            const string dateToParse = "date";
            var parsedDate = new DateTime(1, 2, 3);
            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(dateToParse))
                .Return(parsedDate);
            var recurranceSpecificationFactory = MockRepository.GenerateStub<IBuildRecurranceSpecifications>();
            var recurranceSpecification = MockRepository.GenerateStub<IDecideWhenRecurringChangesOccur>();
            recurranceSpecification
                .Stub(x => x.FirstApplicableDate)
                .Return(parsedDate);
            recurranceSpecificationFactory
                .Stub(x => x.Build(Arg<string>.Is.Anything, Arg<DateTime>.Is.Anything, Arg<DateTime?>.Is.Anything))
                .Return(recurranceSpecification);
            var addChangeAction = new AddChangeAction(walletService, dateParser, recurranceSpecificationFactory);

            const string withdrawalName = "withdrawal for foo";
            var addedChange = new IncomingChange{Amount = 123,ChangeName = withdrawalName};
            var walletDay = new WalletDay {Date = dateToParse, WalletName = walletName};

            addChangeAction.Execute(walletDay, addedChange, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(withdrawalExposer.LastChangeAdded.StartDate, Is.EqualTo(parsedDate));
        }

        [Test]
        public void Execute_gives_built_rule_to_new_change()
        {
            const string walletName = "foo wallet";
            var walletIdentification = new WalletIdentification(walletName);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new AddChangeExposer();
            walletService
                .Stub(x => x.FindRecurringChanger(walletIdentification))
                .Return(withdrawalExposer);
            
            const string dateToParse = "date";
            var parsedDate = new DateTime(1, 2, 3);
            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(dateToParse))
                .Return(parsedDate);

            var recurranceRuleSpecification = MockRepository.GenerateStub<IDecideWhenRecurringChangesOccur>();
            recurranceRuleSpecification
                .Stub(x => x.IsSpecifiedOn(parsedDate))
                .Return(true);

            const string recurranceRule = "foo rule";
            var recurranceSpecificationFactory = MockRepository.GenerateStub<IBuildRecurranceSpecifications>();
            recurranceSpecificationFactory
                .Stub(x => x.Build(recurranceRule, parsedDate, null))
                .Return(recurranceRuleSpecification);

            var addChangeAction = new AddChangeAction(walletService, dateParser, recurranceSpecificationFactory);

            const string withdrawalName = "withdrawal for foo";
            var addedChange = new IncomingChange { Amount = 123, ChangeName = withdrawalName, RecurranceType = recurranceRule};
            var walletDay = new WalletDay { Date = dateToParse, WalletName = walletName };

            addChangeAction.Execute(walletDay, addedChange, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(withdrawalExposer.LastChangeAdded.AppliesTo(parsedDate));
        }
    }
}