using Azure.Core;
using CrypTo.Infrastructure.Contracts.Transactions;
using CrypTo.Infrastructure.Entities.Blockchain;
using CrypTo.Infrastructure.Entities.Transactions;
using CrypTo.Infrastructure.Entities.Wallets;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Mapping;
using CrypTo.Infrastructure.Repositories.Blockchain;
using CrypTo.Infrastructure.Repositories.Transactions;
using CrypTo.Infrastructure.Repositories.Wallets;
using CrypTo.Infrastructure.Services.Cache;
using CrypTo.Infrastructure.Services.RabbitMQ;
using CrypTo.Infrastructure.Services.Transactions;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CrypTo.Bussines.Services.Transactions
{
    public class TransactionService : ITransactionService
    {
        private readonly IRabbitMQService _rabbitMQService;
        private readonly IWalletRepository _walletRepository;
        private readonly IBlockRepository _blockRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ICacheService _cacheService;

        public TransactionService(IRabbitMQService rabbitMQService, IWalletRepository walletRepository,
            IBlockRepository blockRepository, ITransactionRepository transactionRepository,
            ICacheService cacheService)
        {
            _rabbitMQService = rabbitMQService;
            _walletRepository = walletRepository;
            _blockRepository = blockRepository;
            _transactionRepository = transactionRepository;
            _cacheService = cacheService;
        }

        public async Task ProcessTransactionsAsync(List<ProcessTransactionRequest> request)
        {
            var block = await _blockRepository.GetBlockAsync(request.FirstOrDefault()!.BlockIndex).ConfigureAwait(false);

            foreach (var transactionToProcess in request)
            {
                await ProcessTransactionAsync(transactionToProcess, block).ConfigureAwait(false);
            }
        }

        public async Task CreateTransactionAsync(CreateTransactionRequest request)
        {
            var senderWallet = await CheckForExistingWallet(request.SenderAddress!)
               ?? throw new NotFoundException("SenderAddress not found.");

            var receiverWallet = await CheckForExistingWallet(request.ReceiverAddress!)
                ?? throw new NotFoundException("ReceiverAddress not found.");

            var message = $"{request.SenderAddress}:transaction";
            if (!VerifySignature(senderWallet!.PublicKey!, message, request.Signature!))
            {
                throw new BadRequestException("Invalid signature");
            }

            var transaction = request.ToDomain();
            transaction.TransactionId = CalculateHash(transaction);
            
            _cacheService.Set(transaction.TransactionId, transaction);
            await _rabbitMQService.PublishMessageAsync("transactionPool", JsonSerializer.Serialize(new { TransactionId = transaction.TransactionId, Bytes = transaction.Bytes }));
        }

        private async Task ProcessTransactionAsync(ProcessTransactionRequest transactionToProcess, Block block)
        {
            var transactionCache = _cacheService.Get(transactionToProcess.TransactionId!);

            if (transactionCache is null)
            {
                throw new BadRequestException("There is some problem with proccessing the transaction.");
            }

            var transaction = transactionCache as Transaction;
            transaction.Sender = await CheckForExistingWallet(transaction.SenderAddress!);
            transaction.Receiver = await CheckForExistingWallet(transaction.ReceiverAddress!);


            transaction!.Block = block;
            transaction!.Sender!.Balance -= transaction.Amount + transaction.Fee;
            transaction!.Receiver!.Balance += transaction.Amount;

            await _walletRepository.UpdateWalletAsync(transaction!.Sender).ConfigureAwait(false);
            await _walletRepository.UpdateWalletAsync(transaction!.Receiver!).ConfigureAwait(false);

            await _transactionRepository.CreateTransactionAsync(transaction);
        }

        private string CalculateHash(Transaction transaction)
        {
            var transactionData = $"{transaction.Fee}{transaction.Signature}{transaction.Amount}{transaction.ReceiverAddress}{transaction.Timestamp}{transaction.SenderAddress}";
            using (var sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(transactionData))).Replace("-", "").ToLower();
            }
        }

        public async Task<TransactionContract> GetTransactionAsync(string transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionAsync(transactionId).ConfigureAwait(false)
                  ?? throw new NotFoundException("Transaction not found.");

            return transaction.ToContract();
        }

        private bool VerifySignature(string publicKey, string message, string signature)
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

        private async Task<Wallet> CheckForExistingWallet(string walletaddress)
            => await _walletRepository.GetWalletAsync(walletaddress);
    }
}
