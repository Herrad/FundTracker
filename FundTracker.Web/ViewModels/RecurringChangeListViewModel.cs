using System;
using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeListViewModel
    {
        public RecurringChangeListViewModel(IEnumerable<RecurringChangeViewModel> recurringChangeViewModels, DateTime date)
        {
            Date = date;
            RecurringChangeViewModels = recurringChangeViewModels;
        }

        public IEnumerable<RecurringChangeViewModel> RecurringChangeViewModels { get; private set; }
        public DateTime Date { get; private set; }
    }
}