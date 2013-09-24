namespace FundTracker.Web.ViewModels
{
    public class SuccessfullyCreatedWalletViewModel
    {
        public SuccessfullyCreatedWalletViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}