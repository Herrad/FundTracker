using FuncTracker.Web.Controllers;
using NUnit.Framework;

namespace Test.FundTracker.Web.Controllers.Home
{
    [TestFixture]
    public class TestWalletController
    {
        [Test]
        public void Create_returns_ViewResult_with_empty_ViewName()
        {
            var walletController = new WalletController();
            var viewResult = walletController.Create();

            Assert.That(viewResult.ViewName, Is.EqualTo(string.Empty));
        } 
    }
}