using System.Web.Mvc;

namespace FundTracker.Web.Controllers
{
    public interface IAddFundsToWallets
    {
        ActionResult AddFunds(string name, decimal fundsToAdd);
    }
}