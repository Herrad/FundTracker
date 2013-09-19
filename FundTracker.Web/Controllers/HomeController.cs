using System.Web.Mvc;
using FundTracker.Web.Controllers.ValidationFailure;

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
