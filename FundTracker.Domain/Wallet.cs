namespace FundTracker.Domain
{
    public class Wallet : ITakeFundsToAdd, IHaveAvailableFunds
    {
        protected bool Equals(Wallet other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Wallet) obj);
        }
    }
}