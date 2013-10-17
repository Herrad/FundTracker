using System.Web.Mvc;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.ViewModels;

namespace FundTracker.Web.Controllers
{
    public class WalletController : Controller, IAddFundsToWallets, ICreateNewWallets, ICreateRedirects
    {
        private RedirectToRouteResult _redirect;
        private readonly IValidateWalletCreation _createWalletValidation;

        public WalletController(IValidateWalletCreation createWalletValidation)
        {
            _createWalletValidation = createWalletValidation;
        }

        public ViewResult SuccessfullyCreated(string walletName)
        {
            var viewModel = new SuccessfullyCreatedWalletViewModel(walletName);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateWallet(string name)
        {
            _createWalletValidation.ValidateAndRedirect(this, name);
            return _redirect;
        }

        public ViewResult Display(string walletName, decimal availableFunds)
        {
            var walletViewModel = new WalletViewModel(walletName, availableFunds);

            return View("Display", walletViewModel);
        }

        public ActionResult AddFunds(string name, decimal fundsToAdd)
        {
            return RedirectToAction("Display", new { walletName = name, availableFunds = fundsToAdd });
        }

        public ViewResult DisplayNoFunds(string walletName)
        {
            return Display(walletName, 0);
        }

        public void SetRedirect(string action, string controller, object parameters)
        {
            _redirect = RedirectToAction(action, controller, parameters);
        }

    }
}
