namespace CrypTo.Infrastructure.Contracts.Wallets
{
    public class CreateWalletContract
    {
        public string? WalletAddress { get; set; }
        public string? PublicKey { get; set; }
        public string? PrivateKey { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal Balance { get; set; }
    }
}
