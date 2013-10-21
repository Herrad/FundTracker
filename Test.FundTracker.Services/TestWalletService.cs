using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Services
{
    [TestFixture]
    public class TestWalletService : IHaveAListOfWallets
    {
        [Test]
        public void GetBy_throws_an_exception_when_Name_is_not_valid()
        {
            var nameValidater = MockRepository.GenerateStub<IValidateWalletNames>();
            nameValidater.Stub(x => x.IsNameValid("bad name")).Return(false);

            var walletService = new WalletService(this, nameValidater);

            Assert.Throws<ArgumentException>(() => walletService.FindFirstWalletWith("bad name"));
        }
        [Test]
        public void GetBy_should_return_a_wallet_if_a_matching_wallet_exists_in_the_repository()
        {
            var expectedWallet = new Wallet("foo name");

            Wallets = new List<IWallet>{expectedWallet, new Wallet("foo other name")};
            
            var nameValidater = MockRepository.GenerateStub<IValidateWalletNames>();
            nameValidater.Stub(x => x.IsNameValid("foo name")).Return(true);

            var walletService = new WalletService(this, nameValidater);

            var wallet = walletService.FindFirstWalletWith("foo name");

            Assert.That(wallet, Is.EqualTo(expectedWallet));
        }

        [Test]
        public void Add_adds_a_wallet_to_the_WalletsList()
        {
            Wallets = new List<IWallet>();

            var walletService = new WalletService(this, null);

            var wallet = new Wallet("foo name");

            walletService.Add(wallet);

            Assert.That(Wallets.Contains(wallet), "list does not contain wallet");
            Assert.That(Wallets.Count, Is.EqualTo(1));
        }

        public List<IWallet> Wallets { get; private set; }
    }
}
