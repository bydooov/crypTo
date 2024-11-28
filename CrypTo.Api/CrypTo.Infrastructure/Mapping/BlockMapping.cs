using CrypTo.Infrastructure.Contracts.Blockchain;
using CrypTo.Infrastructure.Entities.Blockchain;

namespace CrypTo.Infrastructure.Mapping
{
    public static class BlockMapping
    {
        public static BlockContract ToContract(this Block block)
        {
            return new BlockContract
            {
                CurrentHash = block.CurrentHash,
                Index = block.Index,
                Nonce = block.Nonce,
                PreviousHash = block.PreviousHash,
                Timestamp = block.Timestamp,
            };
        }

        public static Block ToNewBlock(this Block block)
        {
            return new Block
            {
                Index = block.Index + 1,
                Nonce = 0,
                PreviousHash = block.CurrentHash,
                Timestamp = DateTime.Now,
            };
        }
    }
}
