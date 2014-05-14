namespace FundTracker.Web.ViewModels
{
    public class TileViewModel
    {
        public TileViewModel(decimal amount)
        {
            Amount = amount;
        }

        public decimal Amount { get; private set; }
    }
}