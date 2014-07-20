using System;
using System.Collections.Generic;
using System.Globalization;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class CalendarDayViewModelBuilder : IBuildCalanderDayViewModels
    {
        public WalletDatePickerViewModel Build(DateTime selectedDate, WalletIdentification identification)
        {
            var daysInPreviousMonth = BuildDaysInPreviousMonth(selectedDate);

            var daysInNextMonth = BuildDaysInNextMonth(selectedDate);

            var daysInCurrentMonth = BuildDaysInCurrentMonth(selectedDate);

            return new WalletDatePickerViewModel(daysInPreviousMonth, daysInCurrentMonth, daysInNextMonth, selectedDate, identification.Name);
        }

        private static List<string> BuildDaysInCurrentMonth(DateTime selectedDate)
        {
            var daysInCurrentMonth = new List<string>();
            var numberOfDaysInCurrentMonth = GetNumberOfDaysInCurrentMonth(selectedDate);
            for (var i = 1; i <= numberOfDaysInCurrentMonth; i++)
            {
                var dayWithZeroIfNeeded = i.ToString(CultureInfo.InvariantCulture);
                if (i < 10)
                {
                    dayWithZeroIfNeeded = "0" + i;
                }
                daysInCurrentMonth.Add(dayWithZeroIfNeeded);
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
            var newDate = selectedDate.AddMonths(-1);
            var totalDaysInPreviousMonth = DateTime.DaysInMonth(newDate.Year, newDate.Month);
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