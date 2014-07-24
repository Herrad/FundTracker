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
        private readonly ILimitRecurringChanges _recurringChangeLimiter;
        private readonly IRemoveRecurringChanges _changeRemover;

        public RecurringChangeController(
            IBuildRecurringChangeListViewModels recurringChangeListViewModelBuilder, 
            IBuildCreateRecurringChangeViewModels createRecurringChangeViewModelBuilder, 
            IAddRecurringChanges addChangeAction, 
            ILimitRecurringChanges recurringChangeLimiter, 
            IRemoveRecurringChanges changeRemover)
        {
            _recurringChangeListViewModelBuilder = recurringChangeListViewModelBuilder;
            _createRecurringChangeViewModelBuilder = createRecurringChangeViewModelBuilder;
            _addChangeAction = addChangeAction;
            _recurringChangeLimiter = recurringChangeLimiter;
            _changeRemover = changeRemover;
        }

        public ViewResult Display(WalletDay walletDay)
        {
            var recurringChangeListViewModel = _recurringChangeListViewModelBuilder.Build(walletDay.WalletName, walletDay.Date);

            return View(recurringChangeListViewModel);
        }

        public ViewResult CreateWithdrawal(WalletDay walletDay)
        {
            var createRecurringChangeViewModel = _createRecurringChangeViewModelBuilder.Build(walletDay.WalletName, walletDay.Date, "/RecurringChange/AddNewWithdrawal");
            return View("Create", createRecurringChangeViewModel);
        }

        public ViewResult CreateDeposit(WalletDay walletDay)
        {
            var createRecurringChangeViewModel = _createRecurringChangeViewModelBuilder.Build(walletDay.WalletName, walletDay.Date, "/RecurringChange/AddNewDeposit");
            return View("Create", createRecurringChangeViewModel);
        }

        public RedirectToRouteResult AddNewWithdrawal(WalletDay walletDay, IncomingChange incomingChange)
        {
            incomingChange.Amount = 0 - incomingChange.Amount;

            _addChangeAction.Execute(walletDay, incomingChange, this);
            return _redirectResult;
        }

        public RedirectToRouteResult AddNewDeposit(WalletDay walletDay, IncomingChange incomingChange)
        {
            _addChangeAction.Execute(walletDay, incomingChange, this);
            return _redirectResult;
        }

        public void SetRedirect(string action, string controller, object parameters)
        {
            _redirectResult = RedirectToAction(action, controller, parameters);
        }

        public RedirectToRouteResult StopChange(WalletDay walletDay, IncomingChange incomingChange)
        {
            _recurringChangeLimiter.LimitChange(walletDay, incomingChange, this);
            return _redirectResult;
        }

        public RedirectToRouteResult Delete(WalletDay walletDay, IncomingChange incomingChange)
        {
            _changeRemover.Execute(walletDay, incomingChange, this);
            return _redirectResult;
        }
    }
}