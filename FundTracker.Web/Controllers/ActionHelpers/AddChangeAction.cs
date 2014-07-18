using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.ParameterParsers;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class AddChangeAction : IAddRecurringChanges
    {
        private readonly IProvideWallets _walletService;
        private readonly IParseDates _dateParser;

        public AddChangeAction(IProvideWallets walletService, IParseDates dateParser)
        {
            _walletService = walletService;
            _dateParser = dateParser;
        }

        public void Execute(string walletName, string changeName, decimal amount, string date, ICreateRedirects redirecter)
        {
            var walletIdentification = new WalletIdentification(walletName);
            var wallet = _walletService.FindFirstWalletWith(walletIdentification);

            var parsedDate = _dateParser.ParseDateOrUseToday(date);
            wallet.CreateChange(new RecurringChange(changeName, amount, parsedDate));

            redirecter.SetRedirect("Display", "Wallet", new { walletName });
        }
    }
}