namespace FundTracker.Web.ViewModels.Builders
{
    public class CreateRecurringChangeViewModelBuilder : IBuildCreateRecurringChangeViewModels
    {
        public CreateRecurringChangeViewModel Build(string walletName, string selectedDate)
        {
            return new CreateRecurringChangeViewModel(walletName, selectedDate);
        }
    }
}