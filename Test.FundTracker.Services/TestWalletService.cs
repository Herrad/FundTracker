using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Services;
using NUnit.Framework;

namespace Test.FundTracker.Services
{
    [TestFixture]
    public class TestWalletService : IHaveAListOfWallets
    {
        [Test]
        public void GetBy_should_return_a_wallet_if_a_matching_wallet_exists_in_the_repository()
        {
            var expectedWallet = new Wallet("foo name");

            Wallets = new List<Wallet>{expectedWallet, new Wallet("foo other name")};
            var walletService = new WalletService(this);

            var wallet = walletService.GetBy("foo name");

            Assert.That(wallet, Is.EqualTo(expectedWallet));
        }

        [Test]
        public void Add_adds_a_wallet_to_the_WalletsList()
        {
            Wallets = new List<Wallet>();

            var walletService = new WalletService(this);

            var wallet = new Wallet("foo name");

            walletService.Add(wallet);

            Assert.That(Wallets.Contains(wallet), "list does not contain wallet");
            Assert.That(Wallets.Count, Is.EqualTo(1));
        }

        public List<Wallet> Wallets { get; private set; }
    }

    [TestFixture]
    public class TestWalletBuilder
    {
        [Test]
        public void Building_wallet_()
        {

        }
    }
}
