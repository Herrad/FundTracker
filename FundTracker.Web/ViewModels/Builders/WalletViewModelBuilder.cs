using System.Collections.Generic;
using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class WalletViewModelBuilder : IFormatWalletsAsViewModels
    {
        public WalletViewModel FormatWalletAsViewModel(IWallet wallet)
        {
            return new WalletViewModel(wallet.Identification.Name, wallet.AvailableFunds, CreateWithdrawalTilesViewModel(wallet.RecurringChanges));
        }

        private static WithdrawalTilesViewModel CreateWithdrawalTilesViewModel(IEnumerable<RecurringChange> recurringChanges)
        {
            if (recurringChanges == null)
            {
                return null;
            }
            return new WithdrawalTilesViewModel(recurringChanges.Select(x => new WithdrawalTileViewModel(x.Amount)));
        }
    }
}