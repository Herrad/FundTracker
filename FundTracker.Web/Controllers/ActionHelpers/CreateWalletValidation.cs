using System;

namespace FundTracker.Web.Controllers.ActionHelpers
{
    public class CreateWalletValidation : IValidateWalletCreation
    {
        public void ValidateAndRedirect(WalletController controller, string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                controller.SetRedirect("ValidationFailure", "Home", new { failure = "You need to put in a name for this wallet" });
                return;
            }

            controller.SetRedirect("SuccessfullyCreated", "Wallet", new { walletName = name });
        }
    }
}