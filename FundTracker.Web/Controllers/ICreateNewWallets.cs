using System.Web.Mvc;

namespace FundTracker.Web.Controllers
{
    public interface ICreateNewWallets
    {
        [HttpPost]
        ActionResult CreateWallet(string name);
         
    }
}