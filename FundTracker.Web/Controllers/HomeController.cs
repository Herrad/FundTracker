using System.Web.Mvc;
using FundTracker.Web.ViewModels;

namespace FundTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index()
        {
            return View(new HomePageViewModel(null));
        }

        public ViewResult ValidationFailure(string failure)
        {
            var homePageViewModel = new HomePageViewModel(failure);

            return View("Index", homePageViewModel);
        }
    }
}
