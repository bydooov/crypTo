using CrypTo.Infrastructure.Contracts.Blockchain;

namespace CrypTo.Infrastructure.Services.Blockchain
{
    public interface IBlockChainService
    {
        Task<BlockChainContract> GetBlockchainAsync();
        Task<BlockContract> GetBlockAsync(int index);
        Task<BlockChainStatusContract> GetBlockcahinStatusAsync();
    }
}
