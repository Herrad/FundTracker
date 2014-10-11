using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class WalletDatePickerViewModel
    {
        public WalletDatePickerViewModel(List<string> daysInCurrentMonth, DateTime selectedDate, string walletName)
        {
            WalletName = walletName;
            DaysInCurrentMonth = daysInCurrentMonth;
            SelectedDate = selectedDate;
        }

        public List<string> DaysInCurrentMonth { get; private set; }

        public DateTime SelectedDate { get; private set; }
        public string WalletName { get; private set; }
    }
}