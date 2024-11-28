using CrypTo.Infrastructure.Entities.Wallets;

namespace CrypTo.Infrastructure.Repositories.Wallets
{
    public interface IWalletRepository
    {
        Task<Wallet> CreateWalletAsync(Wallet wallet);
        Task<Wallet> GetWalletAsync(string walletAddress);
        Task<Wallet> UpdateWalletAsync(Wallet wallet);
        Task<Wallet> GetWalletTransactionsAsync(string walletAddress);
    }
}
