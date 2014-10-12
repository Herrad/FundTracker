using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildDatePickerDayViewModels
    {
        List<string> Build(DateTime selectedDate);
        IEnumerable<DatePickerDayViewModel> BuildDatePickerDayViewModels(DateTime selectedDate);
    }
}