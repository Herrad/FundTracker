using System.Collections.Generic;

namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeListViewModel
    {
        public RecurringChangeListViewModel(IEnumerable<string> changeNames)
        {
            ChangeNames = changeNames;
        }

        public IEnumerable<string> ChangeNames { get; private set; }
    }
}