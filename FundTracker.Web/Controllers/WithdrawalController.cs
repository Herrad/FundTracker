using System.Web.Mvc;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.Controllers
{
    public class WithdrawalController : Controller
    {
        private readonly IProvideWallets _walletService;

        public WithdrawalController(IProvideWallets walletService)
        {
            _walletService = walletService;
        }

        public ViewResult Create(string walletName)
        {
            return View(new CreateWithdrawalViewModel(walletName));
        }

        public RedirectToRouteResult AddNew(string name, decimal amountToWithdraw)
        {
            var walletIdentification = new WalletIdentification(name);
            var wallet = _walletService.FindFirstWalletWith(walletIdentification);

            wallet.CreateChange(new RecurringChange(walletIdentification, 0-amountToWithdraw));

            return RedirectToAction("Display", "Wallet", new { walletName = name });
        }
    }
}