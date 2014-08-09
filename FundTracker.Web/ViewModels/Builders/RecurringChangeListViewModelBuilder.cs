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
            var stopLinkText = change.GetStopChangeText();
            var stopLinkDestination = GetStopLinkDestination(change, walletName, selectedDate);
            return new RecurringChangeViewModel(change.Name, change.Amount, change.RuleDescription(), stopLinkText, stopLinkDestination, change.Id);
        }

        private static string GetStopLinkDestination(RecurringChange change, string walletName, DateTime selectedDate)
        {
            return "/RecurringChange/StopChange/?walletName=" + walletName + "&date=" + selectedDate.ToString("yyyy-MM-dd") + "&changeId=" + change.Id;
        }
    }
}