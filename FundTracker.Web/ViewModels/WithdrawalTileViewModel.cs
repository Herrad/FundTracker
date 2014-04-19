namespace FundTracker.Web.ViewModels
{
    public class WithdrawalTileViewModel
    {
        public WithdrawalTileViewModel(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; private set; }
    }
}