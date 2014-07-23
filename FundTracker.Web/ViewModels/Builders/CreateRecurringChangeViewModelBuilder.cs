namespace FundTracker.Web.ViewModels.Builders
{
    public class CreateRecurringChangeViewModelBuilder : IBuildCreateRecurringChangeViewModels
    {
        public CreateRecurringChangeViewModel Build(string walletName, string selectedDate, string formAction)
        {
            return new CreateRecurringChangeViewModel(walletName, selectedDate, formAction);
        }
    }
}