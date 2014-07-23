using FundTracker.Web.Controllers.BoundModels;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public interface ILimitRecurringChanges
    {
        void LimitChange(WalletDay walletDay, string changeName, ICreateRedirects redirecter);
    }
}