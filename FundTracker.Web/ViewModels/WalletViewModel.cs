namespace FundTracker.Web.ViewModels
{
    public class WalletViewModel
    {
        public WalletViewModel(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}