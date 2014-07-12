using NUnit.Framework;

namespace Test.Acceptance.FundTracker.Web.Data
{
    [TestFixture]
    public class TestMongoDbAdapter
    {
        [Test]
        public void Can_insert_and_read_from_mongo_db()
        {
            TestDbAdapter.CreateWalletCalled("foo", 100);
            var result = TestDbAdapter.FindWalletCalled("foo");

            Assert.That(result, Is.Not.Null);
        }
    }
}