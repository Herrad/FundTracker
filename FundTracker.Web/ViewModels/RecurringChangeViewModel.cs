namespace FundTracker.Web.ViewModels
{
    public class RecurringChangeViewModel
    {
        public string Name { get; private set; }

        public RecurringChangeViewModel(string name)
        {
            Name = name;
        }
    }
}