using System.Web.Mvc;
using FundTracker.Web.Controllers.ActionHelpers;
using FundTracker.Web.Controllers.BoundModels;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.Controllers
{
    public class RecurringChangeController : Controller, ICreateRedirects
    {
        private readonly IBuildRecurringChangeListViewModels _recurringChangeListViewModelBuilder;
        private readonly IBuildCreateRecurringChangeViewModels _createRecurringChangeViewModelBuilder;
        private RedirectToRouteResult _redirectResult;
        private readonly IAddRecurringChanges _addChangeAction;

        public RecurringChangeController(IBuildRecurringChangeListViewModels recurringChangeListViewModelBuilder, IBuildCreateRecurringChangeViewModels createRecurringChangeViewModelBuilder, IAddRecurringChanges addChangeAction)
        {
            _recurringChangeListViewModelBuilder = recurringChangeListViewModelBuilder;
            _createRecurringChangeViewModelBuilder = createRecurringChangeViewModelBuilder;
            _addChangeAction = addChangeAction;
        }

        public ViewResult Display(WalletDay walletDay)
        {
            var recurringChangeListViewModel = _recurringChangeListViewModelBuilder.Build(walletDay.WalletName, walletDay.Date);

            return View(recurringChangeListViewModel);
        }

        public ViewResult CreateWithdrawal(WalletDay walletDay)
        {
            var createRecurringChangeViewModel = _createRecurringChangeViewModelBuilder.Build(walletDay.WalletName, walletDay.Date);
            return View(createRecurringChangeViewModel);
        }

        public ViewResult CreateDeposit(WalletDay walletDay)
        {
            var createRecurringChangeViewModel = _createRecurringChangeViewModelBuilder.Build(walletDay.WalletName, walletDay.Date);
            return View(createRecurringChangeViewModel);
        }

        public RedirectToRouteResult AddNewWithdrawal(WalletDay walletDay, string withdrawalName, decimal amount)
        {
            _addChangeAction.Execute(walletDay.WalletName, withdrawalName, 0 - amount, walletDay.Date, this);
            return _redirectResult;
        }

        public RedirectToRouteResult AddNewDeposit(WalletDay walletDay, string depositName, decimal amount)
        {
            _addChangeAction.Execute(walletDay.WalletName, depositName, amount, walletDay.Date, this);
            return _redirectResult;
        }

        public void SetRedirect(string action, string controller, object parameters)
        {
            _redirectResult = RedirectToAction(action, controller, parameters);
        }
    }
}