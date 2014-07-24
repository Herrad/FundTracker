using FundTracker.Web.Controllers.BoundModels;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public interface IRemoveRecurringChanges
    {
        void Execute(WalletDay walletDay, string changeName, ICreateRedirects redirecter);
    }
}