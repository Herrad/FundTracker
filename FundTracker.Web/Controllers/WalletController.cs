using System.Web.Mvc;
using FundTracker.Services;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.ViewModels;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.Controllers
{
    public class WalletController : Controller, IShowTheResultOfAddingFundsToAWallet, IShowTheResultOfCreatingNewWallets, ICreateRedirects
    {
        private RedirectToRouteResult _redirect;
        private readonly IRedirectBasedOnWalletCreationValidation _createWalletValidation;
        private readonly IProvideWallets _walletProvider;
        private readonly IFormatWalletsAsViewModels _walletViewModelBuilder;

        public WalletController(IRedirectBasedOnWalletCreationValidation createWalletValidation, IProvideWallets walletProvider, IFormatWalletsAsViewModels walletViewModelBuilder)
        {
            _createWalletValidation = createWalletValidation;
            _walletProvider = walletProvider;
            _walletViewModelBuilder = walletViewModelBuilder;
        }

        public ViewResult SuccessfullyCreated(string walletName)
        {
            var viewModel = new SuccessfullyCreatedWalletViewModel(walletName);

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult CreateWallet(string name)
        {
            _createWalletValidation.ValidateAndCreateWallet(this, name);
            return _redirect;
        }

        public ViewResult Display(string walletName)
        {
            var wallet = _walletProvider.GetBy(walletName);

            var walletViewModel = _walletViewModelBuilder.FormatWalletAsViewModel(wallet);

            return View("Display", walletViewModel);
        }

        public ActionResult AddFunds(string name, decimal fundsToAdd)
        {
            return RedirectToAction("Display", new { walletName = name, availableFunds = fundsToAdd });
        }

        public void SetRedirect(string action, string controller, object parameters)
        {
            _redirect = RedirectToAction(action, controller, parameters);
        }
    }
}
