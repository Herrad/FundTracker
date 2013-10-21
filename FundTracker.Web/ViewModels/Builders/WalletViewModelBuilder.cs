using System.Collections.Generic;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class WalletViewModelBuilder : IFormatWalletsAsViewModels
    {
        public WalletViewModel FormatWalletAsViewModel(IWallet wallet)
        {
            var walletViewModel = new WalletViewModel(wallet.Name, wallet.AvailableFunds, CreateWithdrawalTilesViewModel());
            return walletViewModel;
        }

        private static WithdrawalTilesViewModel CreateWithdrawalTilesViewModel()
        {
            return new WithdrawalTilesViewModel(new List<WithdrawalTileViewModel>());
        }
    }
}