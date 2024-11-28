using CrypTo.Infrastructure.Entities.Transactions;

namespace CrypTo.Infrastructure.Entities.Blockchain
{
    public class Block
    {
        public int Index { get; set; }
        public DateTime Timestamp { get; set; }
        public string? PreviousHash { get; set; }
        public string? CurrentHash { get; set; }
        public int Nonce { get; set; }
        public int Difficulty { get; set; }
        public long BlockTime { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
    }
}

