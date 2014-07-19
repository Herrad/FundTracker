using System;
using System.Security.Cryptography.X509Certificates;
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
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(walletIdentification))
                .Return(withdrawalExposer);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            var addChangeAction = new AddChangeAction(walletService, dateParser, MockRepository.GenerateStub<IBuildRecurranceSpecifications>());

            const string withdrawalName = "withdrawal for foo";
            var addedChange = new AddedChange{Amount = 123,Name = withdrawalName};
            var walletDay = new WalletDay {Date = "date", WalletName = walletName};

            addChangeAction.Execute(walletDay, addedChange, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(withdrawalExposer.WithdrawalAdded, Is.Not.Null);
            Assert.That(withdrawalExposer.WithdrawalAdded.Amount, Is.EqualTo(expectedAmountGivenToWallet));
            Assert.That(withdrawalExposer.WithdrawalAdded.Name, Is.EqualTo(withdrawalName));
        }

        [Test]
        public void Execute_puts_parsed_date_on_wallet()
        {
            const string walletName = "foo wallet";
            var walletIdentification = new WalletIdentification(walletName);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(walletIdentification))
                .Return(withdrawalExposer);

            const string dateToParse = "date";
            var parsedDate = new DateTime(1, 2, 3);
            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(dateToParse))
                .Return(parsedDate);
            var addChangeAction = new AddChangeAction(walletService, dateParser, MockRepository.GenerateStub<IBuildRecurranceSpecifications>());

            const string withdrawalName = "withdrawal for foo";
            var addedChange = new AddedChange{Amount = 123,Name = withdrawalName};
            var walletDay = new WalletDay {Date = dateToParse, WalletName = walletName};

            addChangeAction.Execute(walletDay, addedChange, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(withdrawalExposer.WithdrawalAdded.StartDate, Is.EqualTo(parsedDate));
        }

        [Test]
        public void Execute_gives_built_rule_to_new_change()
        {
            const string walletName = "foo wallet";
            var walletIdentification = new WalletIdentification(walletName);

            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            var withdrawalExposer = new WithdrawalExposer();
            walletService
                .Stub(x => x.FindFirstWalletWith(walletIdentification))
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
                .Stub(x => x.Build(recurranceRule, new DateTime(2001, 2, 3)))
                .Return(recurranceRuleSpecification);

            var addChangeAction = new AddChangeAction(walletService, dateParser, recurranceSpecificationFactory);

            const string withdrawalName = "withdrawal for foo";
            var addedChange = new AddedChange { Amount = 123, Name = withdrawalName, RecurranceRule = recurranceRule};
            var walletDay = new WalletDay { Date = dateToParse, WalletName = walletName };

            addChangeAction.Execute(walletDay, addedChange, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(withdrawalExposer.WithdrawalAdded.AppliesTo(parsedDate));
        }
    }
}