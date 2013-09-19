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

        public ViewResult CreateWallet()
        {
            return View();
        }
    }
}
