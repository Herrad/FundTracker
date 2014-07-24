using FundTracker.Web.Controllers.BoundModels;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public interface ILimitRecurringChanges
    {
        void LimitChange(WalletDay walletDay, IncomingChange incomingChange, ICreateRedirects redirecter);
    }
}