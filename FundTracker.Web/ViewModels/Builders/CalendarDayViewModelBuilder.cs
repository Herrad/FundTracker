using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels.Builders
{
    public class CalendarDayViewModelBuilder : IBuildCalanderDayViewModels
    {
        public CalendarDayViewModel Build(DateTime selectedDate)
        {
            var daysInPreviousMonth = new List<int>();
            var totalDaysInPreviousMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month - 1);
            var dayToGoBackTo = totalDaysInPreviousMonth - 4;
            for (var i = dayToGoBackTo; i <= totalDaysInPreviousMonth; i++)
            {
                daysInPreviousMonth.Add(i);
            }

            var daysInNextMonth = new List<int>();
            var daysInCurrentMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            var dayToGoUpTo = 35 - daysInCurrentMonth;
            for (int i = 1; i <= dayToGoUpTo; i++)
            {
                daysInNextMonth.Add(i);
            }

            return new CalendarDayViewModel(daysInPreviousMonth, new List<int>(), daysInNextMonth, 0);
        }
    }
}