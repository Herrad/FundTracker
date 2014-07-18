namespace FundTracker.Web.Controllers.ActionHelpers
{
    public interface IAddRecurringChanges
    {
        void Execute(string walletName, string changeName, decimal amount, string date, ICreateRedirects redirecter);
    }
}