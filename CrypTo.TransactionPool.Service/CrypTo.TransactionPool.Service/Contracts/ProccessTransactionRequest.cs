namespace CrypTo.TransactionPool.Service.Contracts
{
    public class ProccessTransactionRequest
    {
        public string? TransactionId { get; set; }
        public int BlockIndex { get; set; }
    }
}
