using System;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildWalletDatePickerViewModels
    {
        WalletDatePickerViewModel Build(DateTime selectedDate, WalletIdentification identification);
    }
}