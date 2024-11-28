using CrypTo.Infrastructure.Contracts.Blockchain;
using CrypTo.Infrastructure.Entities.Blockchain;
using CrypTo.Infrastructure.Exceptions;
using CrypTo.Infrastructure.Mapping;
using CrypTo.Infrastructure.Repositories.Blockchain;
using CrypTo.Infrastructure.Services.Blockchain;
using CrypTo.Infrastructure.Services.Cache;
using System.Security.Cryptography;
using System.Text;

namespace CrypTo.Bussines.Services.Blockchain
{
    public class BlockChainService : IBlockChainService
    {
        private readonly IBlockRepository _blockRepository;
        private readonly ICacheService _cacheService;

        public BlockChainService(IBlockRepository blockRepository, ICacheService cacheService)
        {
            _blockRepository = blockRepository;
            _cacheService = cacheService;
        }

        public async Task<BlockChainStatusContract> GetBlockcahinStatusAsync()
        {
            var lastBlock = await _blockRepository.GetLastBlockAsync();
            var pendingTransactions = _cacheService.GetCacheCount();

            return new BlockChainStatusContract
            {
                LastBlock = lastBlock.ToContract(),
                PendingTransactions = pendingTransactions,
                TotalBlocks = lastBlock.Index
            };
        }

        public async Task<BlockContract> GetBlockAsync(int index)
        {
            var block = await _blockRepository.GetBlockAsync(index).ConfigureAwait(false)
            ?? throw new NotFoundException("Block not found.");

            var blockContract = block.ToContract();
            blockContract.Transactions = block.Transactions!.Select(bl => bl.ToContract());

            return blockContract;
        }

        public async Task<BlockChainContract> GetBlockchainAsync()
        {
            var blockchain = await _blockRepository.GetBlockChainAsync().ConfigureAwait(false);

            var blockChainContract = blockchain.Select(b => b.ToContract()).ToList();

            return new BlockChainContract
            {
                Blockchain = blockChainContract
            };
        }

        public string CalculateHash(Block block)
        {
            var blockData = $"{block.Index}{block.Timestamp}{block.PreviousHash}{block.Nonce}";
            using (var sha256 = SHA256.Create())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(blockData))).Replace("-", "").ToLower();
            }
        }
    }
}
