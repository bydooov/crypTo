namespace CrypTo.Infrastructure.Contracts.Transactions
{
    public class CreateTransactionRequest
    {
        public string? SenderAddress { get; set; }
        public string? ReceiverAddress { get; set; }
        public decimal Amount { get; set; }
        public string? Signature { get; set; }
        public decimal Fee { get; set; }
    }
}
