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
    public class TestLimitingRecurringChanges
    {

        [Test]
        public void StopChange_passes_WalletDay_and_changeName_to_limiter()
        {
            var lastApplicableDate = new DateTime(2, 2, 2);
            const string walletName = "foo wallet"; 
            const int changeId = 111;

            var date = lastApplicableDate.ToString("yyyy-MM-dd");
            var walletDay = new WalletDay() { WalletName = walletName, Date = date };

            var wallet = MockRepository.GenerateStub<IHaveRecurringChanges>();
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(wallet);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(date))
                .Return(lastApplicableDate);

            var redirecter = MockRepository.GenerateStub<ICreateRedirects>();

            var recurringChangeLimiter = new RecurringChangeLimiter(walletService, dateParser);
            
            recurringChangeLimiter.LimitChange(walletDay, new IncomingChange(){ChangeId = changeId}, redirecter);

            wallet.AssertWasCalled(x => x.StopChangeOn(changeId, lastApplicableDate), c => c.Repeat.Once());
        }

        [Test]
        public void StopChange_sets_RouteValues_on_controller()
        {
            const string date = "foo date";
            const string walletName = "foo wallet";

            var wallet = MockRepository.GenerateStub<IHaveRecurringChanges>();
            var walletService = MockRepository.GenerateStub<IProvideWallets>();
            walletService
                .Stub(x => x.FindRecurringChanger(new WalletIdentification(walletName)))
                .Return(wallet);

            var dateParser = MockRepository.GenerateStub<IParseDates>();
            dateParser
                .Stub(x => x.ParseDateOrUseToday(date))
                .Return(new DateTime(1, 2, 3));

            var walletDay = new WalletDay() { WalletName = walletName, Date = date };

            var redirecter = MockRepository.GenerateStub<ICreateRedirects>();

            var recurringChangeLimiter = new RecurringChangeLimiter(walletService, dateParser);
            recurringChangeLimiter.LimitChange(walletDay, new IncomingChange(), redirecter);

            redirecter
                .AssertWasCalled(x => x.SetRedirect(Arg<string>.Is.Equal("Display"), Arg<string>.Is.Equal("RecurringChange"), Arg<object>.Is.NotNull));
        }
    }
}