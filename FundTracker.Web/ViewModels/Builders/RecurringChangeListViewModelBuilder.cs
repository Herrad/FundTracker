using System;
using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.ParameterParsers;

namespace FundTracker.Web.ViewModels.Builders
{
    public class RecurringChangeListViewModelBuilder : IBuildRecurringChangeListViewModels
    {
        private readonly IProvideWallets _walletService;
        private readonly IParseDates _dateParser;

        public RecurringChangeListViewModelBuilder(IProvideWallets walletService, IParseDates dateParser)
        {
            _walletService = walletService;
            _dateParser = dateParser;
        }

        public RecurringChangeListViewModel Build(string walletName, string date)
        {
            var wallet = _walletService.FindRecurringChanger(new WalletIdentification(walletName));
            
            var selectedDate = _dateParser.ParseDateOrUseToday(date);

            var applicableChanges = wallet.GetChangesActiveOn(selectedDate);

            return new RecurringChangeListViewModel(applicableChanges.Select(change => BuildRecurringChangeViewModel(change, walletName, selectedDate)), selectedDate, BuildNavigationLinkViewModels(walletName, selectedDate));
        }

        private static IEnumerable<NavigationLinkViewModel> BuildNavigationLinkViewModels(string walletName, DateTime selectedDate)
        {
            return new List<NavigationLinkViewModel>
            {
                new NavigationLinkViewModel("Wallet", "/Wallet/Display/?walletName=" + walletName + "&date=" + selectedDate, "wallet")
            };
        }

        private static RecurringChangeViewModel BuildRecurringChangeViewModel(RecurringChange change, string walletName, DateTime selectedDate)
        {
            return new RecurringChangeViewModel(change.Name, change.Amount, change.RuleDescription(), GetStopLinkText(change), GetStopLinkDestination(change, walletName, selectedDate), change.Id);
        }

        private static string GetStopLinkText(RecurringChange change)
        {
            return IsOneShot(change) ? "Remove from today" : "Stop from today";
        }

        private static bool IsOneShot(RecurringChange change)
        {
            return change.StartDate == change.EndDate;
        }

        private static string GetStopLinkDestination(RecurringChange change, string walletName, DateTime selectedDate)
        {
            var desiredAction = IsOneShot(change) ? "Delete" : "StopChange";
            return "/RecurringChange/" + desiredAction + "/?walletName=" + walletName + "&date=" + selectedDate.ToString("yyyy-MM-dd") + "&changeId=" + change.Id;
        }
    }
}