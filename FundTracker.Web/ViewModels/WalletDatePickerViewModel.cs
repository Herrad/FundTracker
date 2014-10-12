using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class WalletDatePickerViewModel
    {
        public WalletDatePickerViewModel(IEnumerable<DatePickerDayViewModel> daysInCurrentMonth, DateTime selectedDate, string walletName)
        {
            WalletName = walletName;
            DaysInCurrentMonth = daysInCurrentMonth;
            SelectedDate = selectedDate;
        }

        public IEnumerable<DatePickerDayViewModel> DaysInCurrentMonth { get; private set; }

        public DateTime SelectedDate { get; private set; }
        public string WalletName { get; private set; }
    }
}