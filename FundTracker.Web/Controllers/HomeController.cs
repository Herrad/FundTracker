using System.Web.Mvc;
using FundTracker.Web.ViewModels;

namespace FundTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index(string validationMessage)
        {
            var homePageViewModel = new HomePageViewModel(validationMessage);

            return View(homePageViewModel);
        }
    }
}
