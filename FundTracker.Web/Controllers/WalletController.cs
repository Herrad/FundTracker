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
            if (string.IsNullOrEmpty(name))
            {
                return RedirectToAction("Index", "Home", new { failure = new ValidationFailure.ValidationFailure() });
            }

            return RedirectToAction("SuccessfullyCreated");
        }
    }
}
