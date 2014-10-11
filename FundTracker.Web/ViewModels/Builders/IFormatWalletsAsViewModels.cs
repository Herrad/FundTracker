using System;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IFormatWalletsAsViewModels
    {
        WalletViewModel FormatWalletAsViewModel(IKnowAboutAvailableFunds wallet, DateTime dateTime);
    }
}