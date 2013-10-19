using System.Web.Mvc;

namespace FundTracker.Web.Controllers
{
    public interface IShowTheResultOfCreatingNewWallets
    {
        [HttpPost]
        ActionResult CreateWallet(string name);
         
    }
}