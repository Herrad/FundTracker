using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FundTracker.Domain;
using NUnit.Framework;

namespace Test.FundTracker.Domain
{
    [TestFixture]
    public class TestWallet
    {
        [Test]
        public void Adding_funds_to_a_wallet_increments_AvailableFunds()
        {
            const decimal expectedFunds = 150m;
            
            var wallet = new Wallet(null);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(0));

            wallet.AddFunds(expectedFunds);

            Assert.That(wallet.AvailableFunds, Is.EqualTo(expectedFunds));
        }
    }
}
