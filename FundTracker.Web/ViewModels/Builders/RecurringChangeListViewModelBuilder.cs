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

            return new RecurringChangeListViewModel(applicableChanges.Select(change => BuildRecurringChangeViewModel(change, walletName)), selectedDate);
        }

        private static RecurringChangeViewModel BuildRecurringChangeViewModel(RecurringChange change, string walletName)
        {
            return new RecurringChangeViewModel(change.Name, change.Amount, change.StartDate, change.RuleName(), GetStopLinkText(change), GetStopLinkDestination(change, walletName));
        }

        private static string GetStopLinkText(RecurringChange change)
        {
            return IsOneShot(change) ? "Remove from today" : "Stop from today";
        }

        private static bool IsOneShot(RecurringChange change)
        {
            return change.RuleName() == "Just today" || change.StartDate == change.EndDate;
        }

        private static string GetStopLinkDestination(RecurringChange change, string walletName)
        {
            var desiredAction = IsOneShot(change) ? "Delete" : "StopChange";
            return "/RecurringChange/" + desiredAction + "/?walletName=" + walletName + "&date=" + change.StartDate.ToString("yyyy-MM-dd") + "&changeName=" + change.Name;
        }
    }
}