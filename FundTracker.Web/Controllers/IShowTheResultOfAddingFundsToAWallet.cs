using System.Web.Mvc;
using FundTracker.Web.Controllers.BoundModels;

namespace FundTracker.Web.Controllers
{
    public interface IShowTheResultOfAddingFundsToAWallet
    {
        ActionResult AddFunds(WalletDay walletDay, AddedChange addedChange);
    }
}