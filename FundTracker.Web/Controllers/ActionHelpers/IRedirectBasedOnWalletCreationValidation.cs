namespace FundTracker.Web.Controllers.ActionHelpers
{
    public interface IRedirectBasedOnWalletCreationValidation
    {
        void ValidateAndCreateWallet(ICreateRedirects redirectCreater, string name);
    }
}