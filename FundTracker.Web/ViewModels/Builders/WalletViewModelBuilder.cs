using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class WalletViewModelBuilder : IFormatWalletsAsViewModels
    {
        public WalletViewModel FormatWalletAsViewModel(IWallet wallet)
        {
            var walletViewModel = new WalletViewModel(wallet.Identification.Name, wallet.AvailableFunds, CreateWithdrawalTilesViewModel(wallet));
            return walletViewModel;
        }

        private static WithdrawalTilesViewModel CreateWithdrawalTilesViewModel(IHaveRecurringChanges wallet)
        {
            return new WithdrawalTilesViewModel(wallet.RecurringChanges.Select(x => new WithdrawalTileViewModel(x.Amount)));
        }
    }
}