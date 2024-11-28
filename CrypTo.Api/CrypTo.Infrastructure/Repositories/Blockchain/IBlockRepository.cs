using CrypTo.Infrastructure.Entities.Blockchain;

namespace CrypTo.Infrastructure.Repositories.Blockchain
{
    public interface IBlockRepository
    {
        Task<IEnumerable<Block>> GetBlockChainAsync();
        Task<Block> GetBlockAsync(int index);
        Task<Block> GetLastBlockAsync();
        Task AddBlockAsync(Block block);
    }
}
