using FundTracker.Domain;
using FundTracker.Services;
using FundTracker.Web.Controllers.BoundModels;
using FundTracker.Web.Controllers.ParameterParsers;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class RecurringChangeLimiter : ILimitRecurringChanges
    {
        private readonly IProvideWallets _walletService;
        private readonly IParseDates _dateParser;

        public RecurringChangeLimiter(IProvideWallets walletService, IParseDates dateParser)
        {
            _walletService = walletService;
            _dateParser = dateParser;
        }

        public void LimitChange(WalletDay walletDay, string changeName, ICreateRedirects redirecter)
        {
            var parsedDate = _dateParser.ParseDateOrUseToday(walletDay.Date);
            var wallet = _walletService.FindRecurringChanger(new WalletIdentification(walletDay.WalletName));
            wallet.StopChangeOn(changeName, parsedDate);

            redirecter.SetRedirect("Display", "RecurringChange", new {walletName=walletDay.WalletName, date=walletDay.Date});
        }
    }
}