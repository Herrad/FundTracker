using System.Linq;
using System.Web.Mvc;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.Controllers
{
    public class RecurringChangeController : Controller
    {
        private readonly IProvideWallets _walletService;
        private readonly IBuildRecurringChangeListViewModels _recurringChangeListViewModelBuilder;

        public RecurringChangeController(IProvideWallets walletService, IBuildRecurringChangeListViewModels recurringChangeListViewModelBuilder)
        {
            _walletService = walletService;
            _recurringChangeListViewModelBuilder = recurringChangeListViewModelBuilder;
        }

        public ViewResult Display(string walletName)
        {
            var wallet = _walletService.FindFirstWalletWith(new WalletIdentification(walletName));
            var recurringChangeListViewModel = _recurringChangeListViewModelBuilder.Build(wallet);

            return View(recurringChangeListViewModel);
        }

        public ViewResult CreateWithdrawal(string walletName)
        {
            return View(new CreateRecurringChangeViewModel(walletName));
        }

        public ViewResult CreateDeposit(string walletName)
        {
            return View(new CreateRecurringChangeViewModel(walletName));
        }

        public RedirectToRouteResult AddNewWithdrawal(string walletName, string withdrawalName, decimal amount)
        {
            return CreateChange(walletName, withdrawalName, 0 - amount);
        }

        public RedirectToRouteResult AddNewDeposit(string walletName, string depositName, decimal amount)
        {
            return CreateChange(walletName, depositName, amount);
        }

        private RedirectToRouteResult CreateChange(string walletName, string changeName, decimal amount)
        {
            var walletIdentification = new WalletIdentification(walletName);
            var wallet = _walletService.FindFirstWalletWith(walletIdentification);

            wallet.CreateChange(new RecurringChange(changeName, amount));

            return RedirectToAction("Display", "Wallet", new {walletName});
        }
    }
}