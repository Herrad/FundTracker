using FundTracker.Domain;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class CreateWalletValidationRules : IRedirectBasedOnWalletCreationValidation
    {
        private readonly IValidateWalletNames _walletNameValidator;
        private readonly ICreateWallets _walletBuilder;

        public CreateWalletValidationRules(IValidateWalletNames walletNameValidator, ICreateWallets walletBuilder)
        {
            _walletNameValidator = walletNameValidator;
            _walletBuilder = walletBuilder;
        }

        public void ValidateAndCreateWallet(ICreateRedirects redirectCreater, string name)
        {
            if (_walletNameValidator.IsNameValid(name))
            {
                redirectCreater.SetRedirect("SuccessfullyCreated", "Wallet", new { walletName = name });
                _walletBuilder.CreateWallet(new WalletIdentification(name));
                return;
            }
            redirectCreater.SetRedirect("ValidationFailure", "Home", new { failure = "You need to put in a name for this wallet" });
        }
    }
}