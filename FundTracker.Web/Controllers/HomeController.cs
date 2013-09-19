using System.Web.Mvc;

namespace FundTracker.Web.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index(IValidationFailure validationFailure)
        {
            if(validationFailure != null)
            {
                validationFailure.GetFailureMessage();
            }

            return View();
        }
    }
}
