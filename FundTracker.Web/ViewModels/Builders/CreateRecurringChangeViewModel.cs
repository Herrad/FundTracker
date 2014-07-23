namespace FundTracker.Web.ViewModels.Builders
{
    public class CreateRecurringChangeViewModel
    {
        public CreateRecurringChangeViewModel(string walletName, string selectedDate, string formAction)
        {
            SelectedDate = selectedDate;
            WalletName = walletName;
            FormAction = formAction;
        }

        public string WalletName { get; private set; }
        public string SelectedDate { get; private set; }

        public string FormAction { get; private set; }
    }
}