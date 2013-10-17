namespace FundTracker.Web.Controllers.ActionHelpers
{
    public interface IValidateWalletCreation
    {
        void ValidateAndRedirect(WalletController controller, string name);
    }
}