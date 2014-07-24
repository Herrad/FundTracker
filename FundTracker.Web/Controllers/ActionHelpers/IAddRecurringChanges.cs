using FundTracker.Web.Controllers.BoundModels;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public interface IAddRecurringChanges
    {
        void Execute(WalletDay walletDay, IncomingChange incomingChange, ICreateRedirects redirecter);
    }
}