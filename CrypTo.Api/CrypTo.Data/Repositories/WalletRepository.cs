using CrypTo.Infrastructure.Context;
using CrypTo.Infrastructure.Entities.Wallets;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Repositories.Wallets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CrypTo.Data.Repositories
{
    public class WalletRepository : IWalletRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<WalletRepository> _logger;

        public WalletRepository(ApplicationDbContext context, ILogger<WalletRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Wallet> CreateWalletAsync(Wallet wallet)
        {
            try
            {
                var entry = await _context.Wallets.AddAsync(wallet);
                await _context.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<Wallet> GetWalletAsync(string walletAddress)
        {
            try
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletAddress == walletAddress);

                return wallet!;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<Wallet> GetWalletTransactionsAsync(string walletAddress)
        {
            try
            {
                var wallet = await _context.Wallets.Include(w => w.Transactions)
                    .FirstOrDefaultAsync(w => w.WalletAddress == walletAddress);

                return wallet!;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<Wallet> UpdateWalletAsync(Wallet wallet)
        {
            try
            {
                var entry = _context.Wallets.Update(wallet);
                await _context.SaveChangesAsync();

                return entry.Entity;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }
    }
}
