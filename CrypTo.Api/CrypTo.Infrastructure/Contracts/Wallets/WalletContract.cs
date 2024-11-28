using CrypTo.Infrastructure.Contracts.Transactions;

namespace CrypTo.Infrastructure.Contracts.Wallets
{
    public class WalletContract
    {
        public string? WalletAddress { get; set; }
        public string? PublicKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Balance { get; set; }

        public IEnumerable<TransactionContract>? Transactions { get; set; }
    }
}
