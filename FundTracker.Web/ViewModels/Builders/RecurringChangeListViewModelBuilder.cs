using System.Linq;
using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public class RecurringChangeListViewModelBuilder : IBuildRecurringChangeListViewModels
    {
        public RecurringChangeListViewModel Build(IHaveRecurringChanges recurringChanger)
        {
            var changeNames = recurringChanger.RecurringChanges.Select(x => x.Name);

            return new RecurringChangeListViewModel(changeNames);
        }
    }
}