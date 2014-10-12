using System;
using System.Collections.Generic;
using System.Globalization;

namespace FundTracker.Web.ViewModels.Builders
{
    public class DatePickerDayViewModelBuilder : IBuildDatePickerDayViewModels
    {
        public List<string> Build(DateTime selectedDate)
        {
            var daysInCurrentMonth = new List<string>();
            var numberOfDaysInCurrentMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);
            for (var dayOfSelectedMonth = 1; dayOfSelectedMonth <= numberOfDaysInCurrentMonth; dayOfSelectedMonth++)
            {
                var formattedDayWithZero = dayOfSelectedMonth.ToString(CultureInfo.InvariantCulture);
                if (dayOfSelectedMonth < 10)
                {
                    formattedDayWithZero = "0" + dayOfSelectedMonth;
                }
                daysInCurrentMonth.Add(formattedDayWithZero);
            }
            return daysInCurrentMonth;
        }

        public IEnumerable<DatePickerDayViewModel> BuildDatePickerDayViewModels(DateTime selectedDate)
        {
            var datePickerDayViewModels = new List<DatePickerDayViewModel>();
            var numberOfDaysInCurrentMonth = DateTime.DaysInMonth(selectedDate.Year, selectedDate.Month);

            for (var dayOfSelectedMonth = 1; dayOfSelectedMonth <= numberOfDaysInCurrentMonth; dayOfSelectedMonth++)
            {
                datePickerDayViewModels.Add(new DatePickerDayViewModel(FormatDayWithZero(dayOfSelectedMonth)));
            }
            return datePickerDayViewModels;
        }

        private static string FormatDayWithZero(int dayOfSelectedMonth)
        {
            var formattedDayWithZero = dayOfSelectedMonth.ToString(CultureInfo.InvariantCulture);
            if (dayOfSelectedMonth < 10)
            {
                formattedDayWithZero = "0" + dayOfSelectedMonth;
            }
            return formattedDayWithZero;
        }
    }
}