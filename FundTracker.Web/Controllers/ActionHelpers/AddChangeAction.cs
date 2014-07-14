using FundTracker.Domain;
using FundTracker.Services;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class AddChangeAction : IAddRecurringChanges
    {
        private readonly IProvideWallets _walletService;

        public AddChangeAction(IProvideWallets walletService)
        {
            _walletService = walletService;
        }

        public void Execute(string walletName, string changeName, decimal amount, ICreateRedirects redirecter)
        {
            var walletIdentification = new WalletIdentification(walletName);
            var wallet = _walletService.FindFirstWalletWith(walletIdentification);

            wallet.CreateChange(new RecurringChange(changeName, amount));

            redirecter.SetRedirect("Display", "Wallet", new { walletName });
        }
    }
}