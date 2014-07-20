using System;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IFormatWalletsAsViewModels
    {
        WalletViewModel FormatWalletAsViewModel(IHaveRecurringChanges wallet, IHaveChangingFunds fundChanger, DateTime dateTime);
    }
}