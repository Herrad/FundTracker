namespace FundTracker.Web.ViewModels.Builders
{
    public class CreateRecurringChangeViewModelBuilder : IBuildCreateRecurringChangeViewModels
    {
        public CreateRecurringChangeViewModel Build(string walletName)
        {
            return new CreateRecurringChangeViewModel(walletName);
        }
    }
}