using System;
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

        public void Execute(WalletDay walletDay, AddedChange addedChange, ICreateRedirects redirecter)
        {
            var walletIdentification = new WalletIdentification(walletDay.WalletName);
            var wallet = _walletService.FindFirstWalletWith(walletIdentification);

            var firstApplicableDate = _dateParser.ParseDateOrUseToday(walletDay.Date);
            var recurranceSpecification = _recurranceSpecificationFactory.Build(addedChange.RecurranceRule, firstApplicableDate);

            wallet.CreateChange(new RecurringChange(addedChange.Name, addedChange.Amount, firstApplicableDate, recurranceSpecification));

            redirecter.SetRedirect("Display", "Wallet", new { walletDay.WalletName });
        }
    }
}