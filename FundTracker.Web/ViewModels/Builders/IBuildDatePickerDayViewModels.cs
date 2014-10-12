using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildDatePickerDayViewModels
    {
        IEnumerable<DatePickerDayViewModel> BuildDatePickerDayViewModels(DateTime selectedDate);
    }
}