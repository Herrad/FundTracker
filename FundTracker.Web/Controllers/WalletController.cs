using System.Web.Mvc;
using FundTracker.Web.ViewModels;

namespace FundTracker.Web.Controllers
{
    public class WalletController : Controller
    {
        public ViewResult SuccessfullyCreated(string walletName)
        {
            var viewModel = new SuccessfullyCreatedWalletViewModel(walletName);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateWallet(string name)
        {
            return ValidateAndRedirect(name);
        }

        private ActionResult ValidateAndRedirect(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("ValidationFailure", "Home", new { failure = "You need to put in a name for this wallet" });
            }

            return RedirectToAction("SuccessfullyCreated", new { walletName = name});
        }

        public ViewResult Display(string walletName)
        {
            var walletViewModel = new WalletViewModel(walletName);

            return View(walletViewModel);
        }
    }
}
