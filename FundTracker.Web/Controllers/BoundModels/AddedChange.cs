namespace FundTracker.Web.Controllers.BoundModels
{
    public class AddedChange
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public string RecurranceRule { get; set; }
    }
}