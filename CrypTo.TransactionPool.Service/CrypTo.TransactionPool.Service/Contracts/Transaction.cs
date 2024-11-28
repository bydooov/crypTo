namespace CrypTo.TransactionPool.Service.Contracts
{
    public class Transaction
    {
        public string? TransactionId { get; set; }
        public int Bytes { get; set; }
    }
}
