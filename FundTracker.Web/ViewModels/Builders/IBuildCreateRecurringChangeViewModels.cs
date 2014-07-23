namespace FundTracker.Web.ViewModels.Builders
{
    public interface IBuildCreateRecurringChangeViewModels
    {
        CreateRecurringChangeViewModel Build(string walletName, string selectedDate, string formAction);
    }
}