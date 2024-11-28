using CrypTo.Infrastructure.Entities.Blockchain;
using CrypTo.Infrastructure.Mapping;
using CrypTo.Infrastructure.Repositories.Blockchain;
using CrypTo.Infrastructure.Services.Mine;
using CrypTo.Infrastructure.Services.RabbitMQ;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CrypTo.Bussines.Services.Mine
{
    public class MiningService : IMiningService
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IRabbitMQService _rabbitMQService;
        private const int TargetBlockTimeInSeconds = 10;
        private const int DifficultyAdjustmentInterval = 10;

        public MiningService(IBlockRepository blockRepository, IRabbitMQService rabbitMQService)
        {
            _blockRepository = blockRepository;
            _rabbitMQService = rabbitMQService;
        }

        public async Task MineAsync()
        {
            var transactionPoolCount = await _rabbitMQService.GetMessagesCountAsync("transactionPool");

            if (transactionPoolCount > 0)
            {
                var previousBlock = await _blockRepository.GetLastBlockAsync().ConfigureAwait(false);

                var currentBlock = previousBlock.ToNewBlock();
                currentBlock.Difficulty = 5;

                var miningStartTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                MineBlockAsync(currentBlock);
                var miningEndTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                currentBlock.BlockTime = miningEndTime - miningStartTime;

                await _rabbitMQService.PublishMessageAsync("blockPool", JsonSerializer.Serialize(currentBlock));

                await _blockRepository.AddBlockAsync(currentBlock).ConfigureAwait(false);
            }
        }

        private void MineBlockAsync(Block currentBlock)
        {
            string hash = string.Empty;

            while (true)
            {
                currentBlock.Nonce++;
                hash = CalculateBlockHash(currentBlock);

                // Check if the hash meets the difficulty (starts with enough leading zeros)
                if (hash.StartsWith(new string('0', currentBlock.Difficulty)))
                {
                    currentBlock.CurrentHash = hash;
                    break;
                }
            }
        }

        private string CalculateBlockHash(Block block)
        {
            using (var sha256 = SHA256.Create())
            {
                var rawData = $"{block.Index}-{block.PreviousHash}-{block.Timestamp}-{block.Nonce}";
                var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        private async Task<int> CalculateDifficulty(Block lastBlock)
        {
            var blocks = (await _blockRepository.GetBlockChainAsync()).ToList();


            if (blocks.Count < DifficultyAdjustmentInterval)
            {
                return lastBlock.Difficulty; // Keep the same difficulty if not enough blocks have been mined
            }

            long totalBlockTime = 0;
            int blocksToConsider = DifficultyAdjustmentInterval;

            // Calculate the total time taken for the last N blocks
            for (int i = blocks.Count - blocksToConsider; i < blocks.Count; i++)
            {
                totalBlockTime += blocks[i].BlockTime;
            }

            long averageBlockTime = totalBlockTime / blocksToConsider;

            // Adjust difficulty based on the average block time
            if (averageBlockTime < TargetBlockTimeInSeconds)
            {
                return lastBlock.Difficulty + 1; // Increase difficulty if mining time is too fast
            }
            else if (averageBlockTime > TargetBlockTimeInSeconds)
            {
                return lastBlock.Difficulty - 1; // Decrease difficulty if mining time is too slow
            }
            else
            {
                return lastBlock.Difficulty; // Keep the same difficulty if block times are perfect
            }
        }
    }
}
