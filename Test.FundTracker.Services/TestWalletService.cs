﻿using System;
using System.Collections.Generic;
using FundTracker.Domain;
using FundTracker.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace Test.FundTracker.Services
{
    [TestFixture]
    public class TestWalletService : IHaveAListOfWallets, ISaveWallets
    {
        private IWallet _walletSaved;

        [Test]
        public void GetBy_should_return_a_wallet_if_a_matching_wallet_exists_in_the_repository()
        {
            var expectedWallet = new Wallet(new WalletIdentification("foo name"));

            Wallets = new List<IWallet>{expectedWallet, new Wallet(new WalletIdentification("foo other name"))};
            
            var nameValidater = MockRepository.GenerateStub<IValidateWalletNames>();
            nameValidater.Stub(x => x.IsNameValid("foo name")).Return(true);

            var walletService = new WalletService(this, this);

            var wallet = walletService.FindFirstWalletWith(new WalletIdentification("foo name"));

            Assert.That(wallet, Is.EqualTo(expectedWallet));
        }

        [Test]
        public void Add_adds_a_wallet_to_the_WalletsList()
        {
            Wallets = new List<IWallet>();

            var walletService = new WalletService(this, this);

            var wallet = new Wallet(new WalletIdentification("foo name"));

            walletService.Add(wallet);

            Assert.That(Wallets.Contains(wallet), "list does not contain wallet");
            Assert.That(Wallets.Count, Is.EqualTo(1));
        }

        [Test]
        public void Saves_wallet_using_saver()
        {
            _walletSaved = null;
            Wallets = new List<IWallet>();

            var walletService = new WalletService(this, this);

            var wallet = new Wallet(new WalletIdentification("foo name"));

            walletService.Add(wallet);

            Assert.That(_walletSaved, Is.EqualTo(wallet));
        }

        public List<IWallet> Wallets { get; private set; }
        public void Save(IWallet wallet)
        {
            _walletSaved = wallet;
        }
    }
}
