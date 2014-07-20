using FundTracker.Domain;
using FundTracker.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Test.FundTracker.Domain;

namespace Test.FundTracker.Services
{
    [TestFixture]
    public class TestWalletService : IKnowAboutWallets, ISaveWallets
    {
        private IHaveChangingFunds _walletSaved;
        private Wallet _walletToReturnFromGet;

        [Test]
        public void FindRecurringChanger_should_return_a_wallet_if_a_matching_wallet_exists_in_the_repository()
        {
            var expectedWallet = _walletToReturnFromGet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification("foo name"), null);

            var nameValidater = MockRepository.GenerateStub<IValidateWalletNames>();
            nameValidater.Stub(x => x.IsNameValid("foo name")).Return(true);

            var walletService = new WalletService(this, this);

            var wallet = walletService.FindRecurringChanger(new WalletIdentification("foo name"));

            Assert.That(wallet, Is.EqualTo(expectedWallet));
        }

        [Test]
        public void FindFundChanger_should_return_a_wallet_if_a_matching_wallet_exists_in_the_repository()
        {
            var expectedWallet = _walletToReturnFromGet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification("foo name"), null);

            var nameValidater = MockRepository.GenerateStub<IValidateWalletNames>();
            nameValidater.Stub(x => x.IsNameValid("foo name")).Return(true);

            var walletService = new WalletService(this, this);

            var wallet = walletService.FindFundChanger(new WalletIdentification("foo name"));

            Assert.That(wallet, Is.EqualTo(expectedWallet));
        }

        [Test]
        public void Saves_wallet_using_saver()
        {
            _walletSaved = null;

            var walletService = new WalletService(this, this);

            var wallet = new Wallet(new LastEventPublishedReporter(), new WalletIdentification("foo name"), null);

            walletService.Add(wallet);

            Assert.That(_walletSaved, Is.EqualTo(wallet));
        }

        public Wallet Get(WalletIdentification identification)
        {
            return _walletToReturnFromGet;
        }

        public void Save(IHaveChangingFunds wallet)
        {
            _walletSaved = wallet;
        }
    }
}
