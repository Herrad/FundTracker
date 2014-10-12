using System;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class WalletDatePickerViewModelBuilder : IBuildWalletDatePickerViewModels
    {
        private readonly IBuildDatePickerDayViewModels _datePickerDayViewModelBuilder;

        public WalletDatePickerViewModelBuilder(IBuildDatePickerDayViewModels datePickerDayViewModelBuilder)
        {
            _datePickerDayViewModelBuilder = datePickerDayViewModelBuilder;
        }

        public WalletDatePickerViewModel Build(DateTime selectedDate, WalletIdentification identification)
        {
            var daysInCurrentMonth = _datePickerDayViewModelBuilder.BuildDatePickerDayViewModels(selectedDate);

            return new WalletDatePickerViewModel(daysInCurrentMonth, selectedDate, identification.Name);
        }
    }
}