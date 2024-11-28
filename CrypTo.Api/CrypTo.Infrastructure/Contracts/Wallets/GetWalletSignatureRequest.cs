namespace CrypTo.Infrastructure.Contracts.Wallets
{
    public class GetWalletSignatureRequest
    {
        public string? PrivateKey { get; set; }
        public string? Message { get; set; }
    }
}
