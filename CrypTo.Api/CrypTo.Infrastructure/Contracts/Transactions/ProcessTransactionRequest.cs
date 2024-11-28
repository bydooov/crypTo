namespace CrypTo.Infrastructure.Contracts.Transactions
{
    public class ProcessTransactionRequest
    {
        public string? TransactionId { get; set; }
        public int BlockIndex { get; set; }
    }
}
