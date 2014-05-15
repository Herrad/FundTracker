using System;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildCalanderDayViewModels
    {
        CalendarDayViewModel Build(DateTime selectedDate);
    }
}