using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers;
using FundTracker.Web.Controllers.ActionHelpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestWalletCreation
    {
        [Test]
        public void WalletBuilder_is_called_when_name_is_valid()
        {
            const string walletName = "foo name";
            var walletBuilder = MockRepository.GenerateMock<ICreateWallets>();

            var walletNameValidator = MockRepository.GenerateStub<IValidateWalletNames>();
            walletNameValidator
                .Stub(x => x.IsNameValid(walletName))
                .Return(true);

            var walletValidator = new CreateWalletValidation(walletNameValidator,
                                                             walletBuilder);

            walletValidator.ValidateAndCreateWallet(MockRepository.GenerateStub<ICreateRedirects>(), walletName);

            walletBuilder.AssertWasCalled(x => x.CreateWallet(walletName), c => c.Repeat.Once());
        }

        [Test]
        public void WaletBuilder_is_not_called_when_validation_fails()
        {
            const string walletName = "foo name";
            var walletBuilder = MockRepository.GenerateMock<ICreateWallets>();

            var walletNameValidator = MockRepository.GenerateStub<IValidateWalletNames>();
            walletNameValidator
                .Stub(x => x.IsNameValid(walletName))
                .Return(false);

            var walletValidator = new CreateWalletValidation(walletNameValidator,
                                                             walletBuilder);

            walletValidator.ValidateAndCreateWallet(MockRepository.GenerateStub<ICreateRedirects>(), walletName);

            walletBuilder.AssertWasNotCalled(x => x.CreateWallet(walletName), c => c.Repeat.Once());
        }

        [Test]
        public void New_wallet_is_stored()
        {
            const string walletName = "foo name";

            var walletStore = MockRepository.GenerateMock<IStoreCreatedWalets>();

            var walletBuilder = new WalletBuilder(walletStore);

            walletBuilder.CreateWallet(walletName);

            walletStore.AssertWasCalled(x => x.Add(new Wallet(walletName)), c => c.Repeat.Once());
        }
    }
}