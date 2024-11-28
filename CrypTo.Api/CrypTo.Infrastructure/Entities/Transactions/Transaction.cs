using CrypTo.Infrastructure.Entities.Blockchain;
using CrypTo.Infrastructure.Entities.Wallets;

namespace CrypTo.Infrastructure.Entities.Transactions
{
    public class Transaction
    {
        public string? TransactionId { get; set; }
        public virtual Wallet? Sender { get; set; }
        public string? SenderAddress { get; set; }
        public virtual Wallet? Receiver { get; set; }
        public string? ReceiverAddress { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Signature { get; set; }
        public decimal Fee { get; set; }
        public int Bytes { get; set; }
        public int BlockIndex { get; set; }
        public virtual Block? Block { get; set; }
    }
}
