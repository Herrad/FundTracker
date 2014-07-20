using System.Web.Mvc;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.Controllers.ParameterParsers;
using FundTracker.Web.ViewModels;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.Controllers
{
    public class WalletController : Controller, IShowTheResultOfAddingFundsToAWallet, IShowTheResultOfCreatingNewWallets, ICreateRedirects, ICreateViews
    {
        private RedirectToRouteResult _redirect;
        private readonly IRedirectBasedOnWalletCreationValidation _createWalletValidation;
        private readonly IProvideWallets _walletProvider;
        private readonly IFormatWalletsAsViewModels _walletViewModelBuilder;
        private readonly IParseDates _dateParser;

        public WalletController(IRedirectBasedOnWalletCreationValidation createWalletValidation, IProvideWallets walletProvider, IFormatWalletsAsViewModels walletViewModelBuilder, IParseDates dateParser)
        {
            _createWalletValidation = createWalletValidation;
            _walletProvider = walletProvider;
            _walletViewModelBuilder = walletViewModelBuilder;
            _dateParser = dateParser;
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

        public ViewResult Display(string walletName, string date)
        {
            var wallet = _walletProvider.FindFirstWalletWith(new WalletIdentification(walletName));

            var selectedDate = _dateParser.ParseDateOrUseToday(date);
            var walletViewModel = _walletViewModelBuilder.FormatWalletAsViewModel(wallet, wallet, selectedDate);

            return View("Display", walletViewModel);
        }

        public ActionResult AddFunds(string name, decimal fundsToAdd)
        {
            var wallet = _walletProvider.FindFirstWalletWith(new WalletIdentification(name));
            wallet.AddFunds(fundsToAdd);

            return RedirectToAction("Display", new { walletName = name });
        }

        public ActionResult RemoveFunds(string name, decimal fundsToRemove)
        {
            return AddFunds(name, 0 - fundsToRemove);
        }

        public void SetRedirect(string action, string controller, object parameters)
        {
            _redirect = RedirectToAction(action, controller, parameters);
        }
    }
}
