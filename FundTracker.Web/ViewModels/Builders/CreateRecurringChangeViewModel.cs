namespace FundTracker.Web.ViewModels.Builders
{
    public class CreateRecurringChangeViewModel
    {
        public CreateRecurringChangeViewModel(string walletName, string selectedDate)
        {
            SelectedDate = selectedDate;
            WalletName = walletName;
        }

        public string WalletName { get; private set; }
        public string SelectedDate { get; private set; }
    }
}