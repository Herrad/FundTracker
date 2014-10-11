using System.Web.Mvc;
using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;
using FundTracker.Services;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.Controllers.BoundModels;
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
            var fundChanger = _walletProvider.FindFundChanger(new WalletIdentification(walletName));
            
            var selectedDate = _dateParser.ParseDateOrUseToday(date);
            var walletViewModel = _walletViewModelBuilder.FormatWalletAsViewModel(fundChanger, selectedDate);

            return View("Display", walletViewModel);
        }

        public ActionResult AddFunds(WalletDay walletDay, IncomingChange incomingChange)
        {
            var walletName = walletDay.WalletName;
            var dateToApplyTo = _dateParser.ParseDateOrUseToday(walletDay.Date);

            var recurringChanger = _walletProvider.FindRecurringChanger(new WalletIdentification(walletName));

            recurringChanger.CreateChange(incomingChange.ChangeName, incomingChange.Amount, new OneShotRule(dateToApplyTo, null));

            return RedirectToAction("Display", new {walletName });
        }

        public ActionResult RemoveFunds(WalletDay walletDay, IncomingChange incomingChange)
        {
            incomingChange.Amount = 0 - incomingChange.Amount;
            return AddFunds(walletDay, incomingChange);
        }

        public void SetRedirect(string action, string controller, object parameters)
        {
            _redirect = RedirectToAction(action, controller, parameters);
        }
    }
}
