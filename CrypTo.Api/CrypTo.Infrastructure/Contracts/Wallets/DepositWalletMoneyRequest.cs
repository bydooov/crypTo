namespace CrypTo.Infrastructure.Contracts.Wallets
{
    public class DepositWalletMoneyRequest
    {
        public decimal Amount { get; set; }
        public string? Signature { get; set; }
    }
}
