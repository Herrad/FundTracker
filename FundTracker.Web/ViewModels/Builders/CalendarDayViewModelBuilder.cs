using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels.Builders
{
    public class CalendarDayViewModelBuilder : IBuildCalanderDayViewModels
    {
        public CalendarDayViewModel Build(DateTime selectedDate)
        {
            var daysInPreviousMonth = BuildDaysInPreviousMonth(selectedDate);

            var daysInNextMonth = BuildDaysInNextMonth(selectedDate);

            var daysInCurrentMonth = BuildDaysInCurrentMonth(selectedDate);

            return new CalendarDayViewModel(daysInPreviousMonth, daysInCurrentMonth, daysInNextMonth, selectedDate);
        }

        private static List<int> BuildDaysInCurrentMonth(DateTime selectedDate)
        {
            var daysInCurrentMonth = new List<int>();
            var numberOfDaysInCurrentMonth = GetNumberOfDaysInCurrentMonth(selectedDate);
            for (int i = 1; i <= numberOfDaysInCurrentMonth; i++)
            {
                daysInCurrentMonth.Add(i);
            }
            return daysInCurrentMonth;
        }

        private static List<int> BuildDaysInNextMonth(DateTime selectedDate)
        {
            var daysInNextMonth = new List<int>();
            var numberOfDaysInCurrentMonth = GetNumberOfDaysInCurrentMonth(selectedDate);
            var dayToGoUpTo = 35 - numberOfDaysInCurrentMonth;
            for (int i = 1; i <= dayToGoUpTo; i++)
            {
                daysInNextMonth.Add(i);
            }
            return daysInNextMonth;
        }

        private static List<int> BuildDaysInPreviousMonth(DateTime selectedDate)
        {
            var daysInPreviousMonth = new List<int>();
            var totalDaysInPreviousMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month - 1);
            var dayToGoBackTo = totalDaysInPreviousMonth - 4;
            for (var i = dayToGoBackTo; i <= totalDaysInPreviousMonth; i++)
            {
                daysInPreviousMonth.Add(i);
            }
            return daysInPreviousMonth;
        }

        private static int GetNumberOfDaysInCurrentMonth(DateTime selectedDate)
        {
            return DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
        }
    }
}