using CrypTo.Infrastructure.Context;
using CrypTo.Infrastructure.Entities.Blockchain;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Repositories.Blockchain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CrypTo.Data.Repositories
{
    public class BlockRepository : IBlockRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BlockRepository> _logger;

        public BlockRepository(ApplicationDbContext context, ILogger<BlockRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddBlockAsync(Block block)
        {
            try
            {
                await _context.Blockchain.AddAsync(block);

                await _context.SaveChangesAsync();
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<Block> GetBlockAsync(int index)
        {
            try
            {
                var block = await _context.Blockchain.Include(b => b.Transactions).FirstOrDefaultAsync(b => b.Index == index);

                return block!;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<IEnumerable<Block>> GetBlockChainAsync()
        {
            try
            {
                var blockchain = await _context.Blockchain.ToListAsync();

                return blockchain!;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }

        public async Task<Block> GetLastBlockAsync()
        {
            try
            {
                var block = await _context.Blockchain.OrderBy(b => b.Index).LastAsync();

                return block!;
            }
            catch (DatabaseException ex)
            {
                _logger.LogError(ex.Message);

                throw;
            }
        }
    }
}
