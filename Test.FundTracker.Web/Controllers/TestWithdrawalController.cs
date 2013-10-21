using FundTracker.Web.Controllers;
using FundTracker.Web.ViewModels.Builders;
using NUnit.Framework;

namespace Test.FundTracker.Web.Controllers
{
    [TestFixture]
    public class TestWithdrawalController
    {
        [Test]
        public void Create_returns_view_with_empty_ViewName_set()
        {
            var result = new WithdrawalController().Create(null);

            Assert.That(result.ViewName, Is.Empty);
        }

        [Test]
        public void Create_sets_WalletName_on_ViewModel()
        {
            const string walletName = "foo name";
            var result = new WithdrawalController().Create(walletName);

            var model = (CreateWithdrawalViewModel) result.Model;
            Assert.That(model, Is.Not.Null);
            Assert.That(model.WalletName, Is.EqualTo(walletName));
        }

        [Test]
        public void AddNew_redirects_to_Wallet_display_page()
        {
            var result = new WithdrawalController().AddNew("foo name", 123m);

            Assert.That(result.RouteValues["action"], Is.EqualTo("Display"));
            Assert.That(result.RouteValues["controller"], Is.EqualTo("Wallet"));
        }

        [Test]
        public void AddNew_sets_wallet_name_in_RouteValues()
        {
            const string walletName = "foo name";

            var result = new WithdrawalController().AddNew(walletName, 123m);

            Assert.That(result.RouteValues["walletName"], Is.Not.Null);
            Assert.That(result.RouteValues["walletName"], Is.EqualTo(walletName));
        }
    }
}