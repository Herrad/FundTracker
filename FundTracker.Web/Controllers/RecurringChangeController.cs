using System.Web.Mvc;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.Controllers
{
    public class RecurringChangeController : Controller
    {
        private readonly IProvideWallets _walletService;

        public RecurringChangeController(IProvideWallets walletService)
        {
            _walletService = walletService;
        }

        public ViewResult CreateWithdrawal(string walletName)
        {
            return View(new CreateRecurringChangeViewModel(walletName));
        }

        public ViewResult CreateDeposit(string walletName)
        {
            return View(new CreateRecurringChangeViewModel(walletName));
        }

        public RedirectToRouteResult AddNewWithdrawal(string name, decimal amount)
        {
            return CreateChange(name, 0-amount);
        }

        public RedirectToRouteResult AddNewDeposit(string name, decimal amount)
        {
            return CreateChange(name, amount);
        }

        private RedirectToRouteResult CreateChange(string name, decimal amount)
        {
            var walletIdentification = new WalletIdentification(name);
            var wallet = _walletService.FindFirstWalletWith(walletIdentification);

            wallet.CreateChange(new RecurringChange(walletIdentification, amount));

            return RedirectToAction("Display", "Wallet", new {walletName = name});
        }
    }
}