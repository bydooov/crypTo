using CrypTo.Infrastructure.Contracts.Wallets;

namespace CrypTo.Infrastructure.Services.Wallets
{
    public interface IWalletService
    {
        Task<CreateWalletContract> CreateWalletAsync();
        Task<WalletContract> DepositWalletMoneyAsync(string walletAddress, DepositWalletMoneyRequest request);
        Task<WalletContract> GetWalletAsync(string walletAddress);
        Task<WalletContract> GetWalletTransactionsAsync(string walletAddress);
        WalletSignatureContract CreateWalletSignature(string privateKey, string message);
        Task DeleteWalletAsync(string walletAddress, WalletSignatureContract signutare);
    }
}
