using CrypTo.Infrastructure.Context;
using CrypTo.Infrastructure.Entities.Transactions;
using CrypTo.Infrastructure.Entities.Wallets;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Repositories.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CrypTo.Data.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TransactionRepository> _logger;

        public TransactionRepository(ApplicationDbContext context, ILogger<TransactionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreateTransactionAsync(Transaction transaction)
        {
            try
            {
                var entry = await _context.BlockchainTransactions.AddAsync(transaction);

                await _context.SaveChangesAsync();
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<Transaction> GetTransactionAsync(string transactionId)
        {
            try
            {
                var transaction = await _context.BlockchainTransactions.AsNoTracking().FirstOrDefaultAsync(tr => tr.TransactionId == transactionId);

                return transaction;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }
    }
}
