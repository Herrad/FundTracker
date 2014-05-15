using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class CalendarDayViewModel
    {
        public CalendarDayViewModel(List<int> daysInPreviousMonth, List<int> daysInCurrentMonth, List<int> daysInNextMonth, int selectedDay)
        {
            DaysInPreviousMonth = daysInPreviousMonth;
            DaysInCurrentMonth = daysInCurrentMonth;
            DaysInNextMonth = daysInNextMonth;
            SelectedDay = selectedDay;
        }

        public List<int> DaysInPreviousMonth { get; private set; }
        public List<int> DaysInCurrentMonth { get; private set; }
        public List<int> DaysInNextMonth { get; private set; }

        public int SelectedDay { get; private set; }
    }
}