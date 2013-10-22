using System.Collections.Generic;
using FundTracker.Domain;
using NUnit.Framework;

namespace Test.FundTracker.Domain
{
    [TestFixture]
    public class TestWalletIdentification
    {
        [Test]
        public void Identifications_with_the_same_name_are_equal()
        {
            const string walletName = "foo name";

            var identification1 = new WalletIdentification(walletName);
            var identification2 = new WalletIdentification(walletName);

            Assert.That(identification1.Equals(identification2), "identifiers are not the same");
        }
    }
}