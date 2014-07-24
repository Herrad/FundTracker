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

            var applicableChanges = wallet.GetChangesApplicableTo(selectedDate);

            return new RecurringChangeListViewModel(applicableChanges.Select(BuildRecurringChangeViewModel), selectedDate, walletName);
        }

        private static RecurringChangeViewModel BuildRecurringChangeViewModel(RecurringChange change)
        {
            return new RecurringChangeViewModel(change.Name, change.Amount, change.StartDate, change.RuleName(), GetStopLinkText(), GetStopLinkDestination(change));
        }

        private static string GetStopLinkText()
        {
            return "Stop this change";
        }

        private static string GetStopLinkDestination(RecurringChange change)
        {
            return "/RecurringChange/Stop/?walletName=" + "&date=" + change.StartDate.ToString("yyyy-MM-dd") + "&changeName=" + change.Name;
        }
    }
}