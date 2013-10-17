using System.Web.Mvc;

namespace FundTracker.Web.Controllers
{
    public interface IDisplayWallets
    {
        ViewResult Display(string walletName, decimal availableFunds);
        ViewResult DisplayNoFunds(string walletName);
    }
}