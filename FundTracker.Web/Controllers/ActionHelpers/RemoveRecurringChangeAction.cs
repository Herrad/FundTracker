using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.BoundModels;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class RemoveRecurringChangeAction : IRemoveRecurringChanges
    {
        private readonly IProvideWallets _walletService;

        public RemoveRecurringChangeAction(IProvideWallets walletService)
        {
            _walletService = walletService;
        }

        public void Execute(WalletDay walletDay, IncomingChange incomingChange, ICreateRedirects redirecter)
        {
            var wallet = _walletService.FindRecurringChanger(new WalletIdentification(walletDay.WalletName));
            wallet.RemoveChange(incomingChange.ChangeId);
            redirecter.SetRedirect("Display", "RecurringChange", new{walletName = walletDay.WalletName, date = walletDay.Date});
        }
    }
}