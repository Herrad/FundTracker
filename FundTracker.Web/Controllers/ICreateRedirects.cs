namespace FundTracker.Web.Controllers
{
    public interface ICreateRedirects
    {
        void SetRedirect(string action, string controller, object parameters);
    }
}