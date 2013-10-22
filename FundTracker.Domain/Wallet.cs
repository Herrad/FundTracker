namespace FundTracker.Domain
{
    public class Wallet : IWallet
    {
        protected bool Equals(Wallet other)
        {
            return Identification.Equals(other.Identification);
        }

        public override int GetHashCode()
        {
            return (Identification!= null ? Identification.GetHashCode() : 0);
        }

        public Wallet(WalletIdentification walletIdentification)
        {
            Identification = walletIdentification;
        }

        public void AddFunds(decimal fundsToAdd)
        {
            AvailableFunds += fundsToAdd;
        }

        public decimal AvailableFunds { get; private set; }
        public WalletIdentification Identification { get; private set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Wallet) obj);
        }
    }
}