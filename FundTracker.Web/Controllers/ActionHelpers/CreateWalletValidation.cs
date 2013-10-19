using FundTracker.Domain;
using FundTracker.Services;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class CreateWalletValidation : IRedirectBasedOnWalletCreationValidation
    {
        private readonly IValidateWalletNames _walletNameValidator;
        private readonly ICreateWallets _walletBuilder;

        public CreateWalletValidation(IValidateWalletNames walletNameValidator, ICreateWallets walletBuilder)
        {
            _walletNameValidator = walletNameValidator;
            _walletBuilder = walletBuilder;
        }

        public void ValidateAndCreateWallet(ICreateRedirects redirectCreater, string name)
        {
            if (_walletNameValidator.IsNameValid(name))
            {
                redirectCreater.SetRedirect("SuccessfullyCreated", "Wallet", new { walletName = name });
                _walletBuilder.CreateWallet(name);
                return;
            }
            redirectCreater.SetRedirect("ValidationFailure", "Home", new { failure = "You need to put in a name for this wallet" });
        }
    }
}