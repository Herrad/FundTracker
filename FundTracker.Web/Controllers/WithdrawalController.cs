using System.Web.Mvc;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.Controllers
{
    public class WithdrawalController : Controller
    {
        public ViewResult Create(string walletName)
        {
            return View(new CreateWithdrawalViewModel(walletName));
        }

        public RedirectToRouteResult AddNew(string name, decimal amount)
        {
            return RedirectToAction("Display", "Wallet", new { walletName = name });
        }
    }
}