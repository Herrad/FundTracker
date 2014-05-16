using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class CalendarDayViewModel
    {
        public CalendarDayViewModel(List<int> daysInPreviousMonth, List<int> daysInCurrentMonth, List<int> daysInNextMonth, DateTime selectedDate)
        {
            DaysInPreviousMonth = daysInPreviousMonth;
            DaysInCurrentMonth = daysInCurrentMonth;
            DaysInNextMonth = daysInNextMonth;
            SelectedDate = selectedDate;
        }

        public List<int> DaysInPreviousMonth { get; private set; }
        public List<int> DaysInCurrentMonth { get; private set; }
        public List<int> DaysInNextMonth { get; private set; }

        public DateTime SelectedDate { get; private set; }
    }
}