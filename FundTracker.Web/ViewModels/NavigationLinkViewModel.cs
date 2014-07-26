namespace FundTracker.Web.ViewModels
{
    public class NavigationLinkViewModel
    {
        public string LinkText { get; private set; }
        public string Target { get; private set; }
        public string LinkClass { get; private set; }

        public NavigationLinkViewModel(string linkText, string target, string linkClass)
        {
            LinkClass = linkClass;
            LinkText = linkText;
            Target = target;
        }
    }
}