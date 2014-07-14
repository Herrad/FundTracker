using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers;
using FundTracker.Web.Controllers.ActionHelpers;
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

            var addChangeAction = new AddChangeAction(walletService);

            const string withdrawalName = "withdrawal for foo";
            addChangeAction.Execute(walletName, withdrawalName, 123, MockRepository.GenerateStub<ICreateRedirects>());

            Assert.That(withdrawalExposer.WithdrawalAdded, Is.Not.Null);
            Assert.That(withdrawalExposer.WithdrawalAdded.Amount, Is.EqualTo(expectedAmountGivenToWallet));
            Assert.That(withdrawalExposer.WithdrawalAdded.Name, Is.EqualTo(withdrawalName));
        }
    }
}