using System.Linq;
using FundTracker.Domain;
using FundTracker.Services;

namespace FundTracker.Web.ViewModels.Builders
{
    public class RecurringChangeListViewModelBuilder : IBuildRecurringChangeListViewModels
    {
        private readonly IProvideWallets _walletService;

        public RecurringChangeListViewModelBuilder(IProvideWallets walletService)
        {
            _walletService = walletService;
        }

        public RecurringChangeListViewModel Build(string walletName)
        {
            var wallet = _walletService.FindFirstWalletWith(new WalletIdentification(walletName));

            var changeNames = wallet.RecurringChanges.Select(x => x.Name);

            return new RecurringChangeListViewModel(changeNames);
        }
    }
}