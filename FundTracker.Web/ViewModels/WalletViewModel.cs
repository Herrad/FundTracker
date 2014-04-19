using System.Collections;
using System.Web.Mvc;

namespace FundTracker.Web.ViewModels
{
    public class WalletViewModel
    {
        public WalletViewModel(string name, decimal availableFunds, WithdrawalTilesViewModel withdrawalTilesViewModel)
        {
            WithdrawalTilesViewModel = withdrawalTilesViewModel;
            AvailableFunds = availableFunds;
            Name = name;
        }

        public string Name { get; private set; }

        public decimal AvailableFunds { get; private set; }

        public WithdrawalTilesViewModel WithdrawalTilesViewModel { get; private set; }
    }
}