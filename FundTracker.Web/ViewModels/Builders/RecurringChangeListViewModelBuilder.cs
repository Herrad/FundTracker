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
            var wallet = _walletService.FindFirstWalletWith(new WalletIdentification(walletName));
            
            var selectedDate = _dateParser.ParseDateOrUseToday(date);

            var changeNames = wallet.RecurringChanges.Where(x => x.AppliesTo(selectedDate)).Select(x => x.Name);

            return new RecurringChangeListViewModel(changeNames);
        }
    }
}