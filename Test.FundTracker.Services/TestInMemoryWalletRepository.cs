using FundTracker.Services;
using NUnit.Framework;

namespace Test.FundTracker.Services
{
    [TestFixture]
    public class TestInMemoryWalletRepository
    {
        [Test]
        public void Wallet_list_is_not_null()
        {
            var walletRepository = new InMemoryWalletRepository();

            Assert.That(walletRepository.Wallets, Is.Not.Null);
        }
    }
}