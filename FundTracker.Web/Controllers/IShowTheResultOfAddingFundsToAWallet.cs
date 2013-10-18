using System.Web.Mvc;

namespace FundTracker.Web.Controllers
{
    public interface IShowTheResultOfAddingFundsToAWallet
    {
        ActionResult AddFunds(string name, decimal fundsToAdd);
    }
}