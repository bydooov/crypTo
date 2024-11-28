using CrypTo.Infrastructure.Contracts.Transactions;

namespace CrypTo.Infrastructure.Services.Transactions
{
    public interface ITransactionService
    {
        Task CreateTransactionAsync(CreateTransactionRequest request);
        Task ProcessTransactionsAsync(List<ProcessTransactionRequest> request);
        Task<TransactionContract> GetTransactionAsync(string transactionId);
    }
}
