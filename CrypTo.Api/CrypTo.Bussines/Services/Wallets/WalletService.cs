using CrypTo.Infrastructure.Contracts.Wallets;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Mapping;
using CrypTo.Infrastructure.Repositories.Wallets;
using CrypTo.Infrastructure.Services.Wallets;
using System.Security.Cryptography;
using System.Text;

namespace CrypTo.Bussines.Services.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IWalletRepository _walletRepository;

        public WalletService(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<CreateWalletContract> CreateWalletAsync()
        {
            var wallet = WalletMapping.ToDomain();

            using var ecdsa = ECDsa.Create();

            wallet.PublicKey = Convert.ToBase64String(ecdsa.ExportSubjectPublicKeyInfo());
            wallet.WalletAddress = GetAddressFromPublicKey(wallet.PublicKey);

            var privateKey = Convert.ToBase64String(ecdsa.ExportECPrivateKey());
            var privateKeyHashBytes = ComputeSha256Hash(privateKey);
            wallet.PrivateKey = Convert.ToBase64String(privateKeyHashBytes);

            var createdWallet = await _walletRepository.CreateWalletAsync(wallet).ConfigureAwait(false);

            var walletContract = createdWallet.ToCreateContract();
            walletContract.PrivateKey = privateKey;

            return walletContract;
        }

        public async Task<WalletContract> DepositWalletMoneyAsync(string walletAddress, DepositWalletMoneyRequest request)
        {
            var wallet = await _walletRepository.GetWalletAsync(walletAddress!).ConfigureAwait(false);

            if (wallet is null || wallet.IsDeleted)
            {
                throw new NotFoundException("Wallet not found.");
            }

            string message = $"{walletAddress}:{request.Amount}";
            if (!VerifySignature(wallet!.PublicKey!, message, request.Signature!))
            {
                throw new BadRequestException("Invalid signature");
            }

            wallet.Balance += request.Amount;

            var updatedWallet = await _walletRepository.UpdateWalletAsync(wallet).ConfigureAwait(false);

            return updatedWallet.ToContract();
        }

        public WalletSignatureContract CreateWalletSignature(string privateKey, string message)
        {
            byte[] privateKeyBytes = Convert.FromBase64String(privateKey);

            using var ecdsa = ECDsa.Create();
            ecdsa.ImportECPrivateKey(privateKeyBytes, out _);

            // Convert the message to bytes and sign it
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            byte[] signatureBytes = ecdsa.SignData(messageBytes, HashAlgorithmName.SHA256);

            var signature = Convert.ToBase64String(signatureBytes);

            return new WalletSignatureContract { Signature = signature };
        }

        public async Task<WalletContract> GetWalletAsync(string walletAddress)
        {
            var wallet = await _walletRepository.GetWalletAsync(walletAddress!).ConfigureAwait(false);

            return wallet is null || wallet.IsDeleted ? throw new NotFoundException("Wallet not found.") : wallet.ToContract();
        }

        public async Task DeleteWalletAsync(string walletAddress, WalletSignatureContract signutare)
        {
            var wallet = await _walletRepository.GetWalletAsync(walletAddress!).ConfigureAwait(false);
            if (wallet is null || wallet.IsDeleted)
            {
                throw new NotFoundException("Wallet not found.");
            }

            string message = $"{walletAddress}:delete";
            if (!VerifySignature(wallet!.PublicKey!, message, signutare.Signature!))
            {
                throw new BadRequestException("Invalid signature");
            }

            wallet.IsDeleted = true;
            await _walletRepository.UpdateWalletAsync(wallet).ConfigureAwait(false);
        }

        public async Task<WalletContract> GetWalletTransactionsAsync(string walletAddress)
        {
            var wallet = await _walletRepository.GetWalletTransactionsAsync(walletAddress!).ConfigureAwait(false);

            if (wallet is null || wallet.IsDeleted)
            {
                throw new NotFoundException("Wallet not found.");
            }

            var walletContract = wallet.ToContract();
            walletContract.Transactions = wallet.Transactions!.Select(tr => tr.ToContract());

            return walletContract;
        }

        private string GetAddressFromPublicKey(string publicKey)
        {
            using var sha256 = SHA256.Create();
            byte[] publicKeyBytes = Encoding.UTF8.GetBytes(publicKey);
            byte[] hash = sha256.ComputeHash(publicKeyBytes);
            return Convert.ToBase64String(hash).Substring(0, 16);
        }

        private byte[] ComputeSha256Hash(string data)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashBytes = sha256.ComputeHash(Convert.FromBase64String(data));
            return hashBytes;
        }

        public bool VerifySignature(string publicKey, string message, string signature)
        {
            // Decode public key
            byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

            using var ecdsa = ECDsa.Create();
            ecdsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);

            // Decode signature
            byte[] signatureBytes = Convert.FromBase64String(signature);

            // Verify the signature
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            return ecdsa.VerifyData(messageBytes, signatureBytes, HashAlgorithmName.SHA256);
        }
    }
}
