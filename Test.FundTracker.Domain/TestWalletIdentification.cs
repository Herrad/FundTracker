using System.Collections.Generic;
using FundTracker.Domain;
using NUnit.Framework;

namespace Test.FundTracker.Domain
{
    [TestFixture]
    public class TestWalletIdentification
    {
        [Test]
        public void Identifies_first_wallet_in_a_list_matching_name()
        {
            const string name = "foo wallet";

            var expectedWallet = new Wallet("foo wallet");
            var listOfWallets = new List<IWallet>
                {
                    expectedWallet,
                    new Wallet("foo name"),
                    new Wallet("foo other name"),
                    expectedWallet
                };

            var identifier = new WalletNameIdentifier(name);

            var wallet = identifier.FindFirstMatchingWalletIn(listOfWallets);

            Assert.That(wallet, Is.EqualTo(expectedWallet));
        }
    }
}