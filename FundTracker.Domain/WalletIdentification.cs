namespace FundTracker.Domain
{
    public class WalletIdentification
    {
        protected bool Equals(WalletIdentification other)
        {
            return string.Equals(Name, other.Name);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public WalletIdentification(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override bool Equals(object obj)
        {
            var other = (WalletIdentification) obj;
            return Equals(other);
        }
    }
}