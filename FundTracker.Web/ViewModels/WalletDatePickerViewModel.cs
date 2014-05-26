using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class WalletDatePickerViewModel
    {
        public WalletDatePickerViewModel(List<int> daysInPreviousMonth, List<string> daysInCurrentMonth, List<int> daysInNextMonth, DateTime selectedDate, string walletName)
        {
            WalletName = walletName;
            DaysInPreviousMonth = daysInPreviousMonth;
            DaysInCurrentMonth = daysInCurrentMonth;
            DaysInNextMonth = daysInNextMonth;
            SelectedDate = selectedDate;
        }

        public List<int> DaysInPreviousMonth { get; private set; }
        public List<string> DaysInCurrentMonth { get; private set; }
        public List<int> DaysInNextMonth { get; private set; }

        public DateTime SelectedDate { get; private set; }
        public string WalletName { get; private set; }
    }
}