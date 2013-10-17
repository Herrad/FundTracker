using FundTracker.Services;
using NUnit.Framework;

namespace Test.FundTracker.Services
{
    [TestFixture]
    public class TestWalletService
    {
        [Test]
        public void Get_should_return_a_wallet()
        {
            var walletService = new WalletService();

            var wallet = walletService.Get();

            Assert.That(wallet, Is.Not.Null);
        }
    }
}
