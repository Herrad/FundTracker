namespace FundTracker.Web.Controllers.BoundModels
{
    public class AddedChange
    {
        public string ChangeName { get; set; }
        public decimal Amount { get; set; }

        public string RecurranceRule { get; set; }
    }
}