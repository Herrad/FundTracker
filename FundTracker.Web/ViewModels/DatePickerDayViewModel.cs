namespace FundTracker.Web.ViewModels
{
    public class DatePickerDayViewModel
    {
        public DatePickerDayViewModel(string text)
        {
            Text = text;
        }

        public string Text { get; private set; }
    }
}