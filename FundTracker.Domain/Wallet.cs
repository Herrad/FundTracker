namespace FundTracker.Domain
{
    public class Wallet : ITakeFundsToAdd, IHaveAvailableFunds
    {
        public Wallet(string name)
        {
            Name = name;
        }

        public void AddFunds(decimal fundsToAdd)
        {
            AvailableFunds += fundsToAdd;
        }

        public decimal AvailableFunds { get; private set; }
        public string Name { get; private set; }
    }
}