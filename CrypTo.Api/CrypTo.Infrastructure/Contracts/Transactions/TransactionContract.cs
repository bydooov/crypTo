namespace CrypTo.Infrastructure.Contracts.Transactions
{
    public class TransactionContract
    {
        public string? TransactionId { get; set; }
        public string? SenderAddress { get; set; }
        public string? ReceiverAddress { get; set; }
        public decimal Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public decimal Fee { get; set; }
        public int BlockIndex { get; set; }
    }
}
