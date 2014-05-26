using System;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildCalanderDayViewModels
    {
        WalletDatePickerViewModel Build(DateTime selectedDate, WalletIdentification identification);
    }
}