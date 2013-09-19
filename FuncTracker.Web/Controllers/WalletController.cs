using System.Web.Mvc;

namespace FuncTracker.Web.Controllers
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
                return RedirectToAction("Index", "Home");
            return View();
        }
    }
}
