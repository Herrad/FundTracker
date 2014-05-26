using System;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IFormatWalletsAsViewModels
    {
        WalletViewModel FormatWalletAsViewModel(IWallet wallet, DateTime dateTime);
    }
}