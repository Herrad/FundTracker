using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Services;
using NUnit.Framework;

namespace Test.FundTracker.Services
{
    [TestFixture]
    public class TestWalletService : IHaveAListOfWallets
    {
        private List<Wallet> _repositoryWallets;

        [Test]
        public void GetBy_should_return_a_wallet_if_a_matching_wallet_exists_in_the_repository()
        {
            var expectedWallet = new Wallet("foo name");

            _repositoryWallets = new List<Wallet>{expectedWallet, new Wallet("foo other name")};
            var walletService = new WalletService(this);

            var wallet = walletService.GetBy("foo name");

            Assert.That(wallet, Is.EqualTo(expectedWallet));
        }

        public List<Wallet> Wallets { get { return _repositoryWallets; } }
    }
}
