using CrypTo.Infrastructure.Entities.Transactions;

namespace CrypTo.Infrastructure.Repositories.Transactions
{
    public interface ITransactionRepository
    {
        Task CreateTransactionAsync(Transaction transaction);
        Task<Transaction> GetTransactionAsync(string transactionId);
    }
}
