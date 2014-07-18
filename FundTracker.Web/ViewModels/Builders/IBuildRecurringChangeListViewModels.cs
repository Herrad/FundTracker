namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildRecurringChangeListViewModels
    {
        RecurringChangeListViewModel Build(string walletName, string date);
    }
}