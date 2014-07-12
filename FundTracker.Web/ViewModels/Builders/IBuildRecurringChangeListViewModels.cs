using FundTracker.Domain;

namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildRecurringChangeListViewModels
    {
        RecurringChangeListViewModel Build(IHaveRecurringChanges recurringChanger);
    }
}