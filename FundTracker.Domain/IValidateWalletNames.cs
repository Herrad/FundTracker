namespace FundTracker.Domain
{
    public interface IValidateWalletNames
    {
        bool IsNameValid(string name);
    }
}