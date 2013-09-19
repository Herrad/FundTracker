using System.Web.Mvc;

namespace FundTracker.Web.Controllers
{
    public class WalletController : Controller
    {
        //
        // GET: /Wallet/

        public ViewResult SuccessfullyCreated()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateWallet(string name)
        {
            return ValidateAndRedirect(name);
        }

        private ActionResult ValidateAndRedirect(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var validationFailure = CreateNoNameValidationFailure();
                return RedirectToAction("Index", "Home", new {failure = validationFailure});
            }

            return RedirectToAction("SuccessfullyCreated");
        }

        private static ValidationFailure CreateNoNameValidationFailure()
        {
            return new ValidationFailure("You need to put in a name for this wallet");
        }
    }
}
