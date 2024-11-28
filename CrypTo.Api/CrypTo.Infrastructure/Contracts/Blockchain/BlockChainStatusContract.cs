namespace CrypTo.Infrastructure.Contracts.Blockchain
{
    public class BlockChainStatusContract
    {
        public int TotalBlocks { get; set; }
        public int PendingTransactions { get; set; }
        public BlockContract? LastBlock { get; set; }
    }
}
