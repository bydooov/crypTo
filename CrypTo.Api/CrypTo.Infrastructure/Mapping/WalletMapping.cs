using CrypTo.Infrastructure.Contracts.Wallets;
using CrypTo.Infrastructure.Entities.Wallets;

namespace CrypTo.Infrastructure.Mapping
{
    public static class WalletMapping
    {
        public static Wallet ToDomain()
        {
            return new Wallet
            {
                Balance = 0,
                IsDeleted = false
            };
        }

        public static CreateWalletContract ToCreateContract(this Wallet wallet)
        {
            return new CreateWalletContract
            {
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt,
                PublicKey = wallet.PublicKey,
                WalletAddress = wallet.WalletAddress
            };
        }

        public static WalletContract ToContract(this Wallet wallet)
        {
            return new WalletContract
            {
                Balance = wallet.Balance,
                CreatedAt = wallet.CreatedAt,
                PublicKey = wallet.PublicKey,
                WalletAddress = wallet.WalletAddress
            };
        }
    }
}
