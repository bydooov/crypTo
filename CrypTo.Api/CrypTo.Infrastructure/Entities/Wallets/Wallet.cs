using CrypTo.Infrastructure.Entities.Transactions;

namespace CrypTo.Infrastructure.Entities.Wallets
{
    public class Wallet
    {
        public string? WalletAddress { get; set; }
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}
