using FundTracker.Domain;
using FundTracker.Domain.RecurranceRules;
using FundTracker.Services;
using FundTracker.Web.Controllers.BoundModels;
using FundTracker.Web.Controllers.ParameterParsers;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class AddChangeAction : IAddRecurringChanges
    {
        private readonly IProvideWallets _walletService;
        private readonly IParseDates _dateParser;
        private readonly IBuildRecurranceSpecifications _recurranceSpecificationFactory;

        public AddChangeAction(IProvideWallets walletService, IParseDates dateParser, IBuildRecurranceSpecifications recurranceSpecificationFactory)
        {
            _walletService = walletService;
            _dateParser = dateParser;
            _recurranceSpecificationFactory = recurranceSpecificationFactory;
        }

        public void Execute(WalletDay walletDay, IncomingChange incomingChange, ICreateRedirects redirecter)
        {
            var walletIdentification = new WalletIdentification(walletDay.WalletName);
            var wallet = _walletService.FindRecurringChanger(walletIdentification);

            var firstApplicableDate = _dateParser.ParseDateOrUseToday(walletDay.Date);
            var recurranceSpecification = _recurranceSpecificationFactory.Build(incomingChange.RecurranceType, firstApplicableDate, null);

            wallet.CreateChange(incomingChange.ChangeName, incomingChange.Amount, recurranceSpecification);

            redirecter.SetRedirect("Display", "Wallet", new { walletDay.WalletName });
        }
    }
}