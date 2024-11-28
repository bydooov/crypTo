using CrypTo.Infrastructure.Contracts.Transactions;

namespace CrypTo.Infrastructure.Contracts.Blockchain
{
    public class BlockContract
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public string? PreviousHash { get; set; }
        public string? CurrentHash { get; set; }
        public int Nonce { get; set; }
        public IEnumerable<TransactionContract>? Transactions { get; set; }
    }
}
