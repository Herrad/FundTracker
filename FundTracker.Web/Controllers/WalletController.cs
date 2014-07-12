using System;
using System.Globalization;
using System.Web.Mvc;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.ActionHelpers;
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

        public ViewResult Display(string walletName, string date)
        {
            var wallet = _walletProvider.FindFirstWalletWith(new WalletIdentification(walletName));

            var selectedDate = SetDateCookie(date);
            var walletViewModel = _walletViewModelBuilder.FormatWalletAsViewModel(wallet, selectedDate);

            return View("Display", walletViewModel);
        }

        private DateTime SetDateCookie(string date)
        {
            DateTime selectedDate;
            var couldParse = DateTime.TryParseExact(date, "dd-MM-yy", new DateTimeFormatInfo(), DateTimeStyles.None,
                out selectedDate);
            if (!couldParse)
            {
                selectedDate = DateTime.Today;
            }
            return selectedDate;
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
