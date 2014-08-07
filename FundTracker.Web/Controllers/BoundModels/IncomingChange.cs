namespace FundTracker.Web.Controllers.BoundModels
{
    public class IncomingChange
    {
        public string ChangeName { get; set; }
        public decimal Amount { get; set; }

        public string RecurranceType { get; set; }
        public int ChangeId { get; set; }
    }
}