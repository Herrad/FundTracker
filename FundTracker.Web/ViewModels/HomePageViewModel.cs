namespace FundTracker.Web.ViewModels
{
    public class HomePageViewModel
    {
        public HomePageViewModel(string validationFailure)
        {
            ValidationFailure = validationFailure;
        }

        public string ValidationFailure { get; private set; }
    }
}