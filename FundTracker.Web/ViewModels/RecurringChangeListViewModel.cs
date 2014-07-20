using System.Collections.Generic;
using FundTracker.Web.ViewModels.Builders;

namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeListViewModel
    {
        public RecurringChangeListViewModel(IEnumerable<RecurringChangeViewModel> recurringChangeViewModels)
        {
            RecurringChangeViewModels = recurringChangeViewModels;
        }

        public IEnumerable<RecurringChangeViewModel> RecurringChangeViewModels { get; private set; }
    }
}