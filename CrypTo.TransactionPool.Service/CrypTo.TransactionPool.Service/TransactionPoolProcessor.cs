using CrypTo.TransactionPool.Service.Contracts;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace CrypTo.TransactionPool.Service
{
    public class TransactionPoolProcessor
    {
        private readonly ConnectionFactory _factory;

        public TransactionPoolProcessor()
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public async Task<List<ProccessTransactionRequest>> ProcessTransactionsAsync(int currentBlock)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var transactions = await channel.QueueDeclarePassiveAsync("transactionPool");

            var transactionCount = (int)transactions.MessageCount;

            List<Transaction> transactionsList = [];
            Queue<BasicGetResult> getResultQueue = new();
            Dictionary<BasicGetResult, Transaction> mappedResult = new();

            for (int i = 0; i < transactionCount; i++)
            {
                var result = await channel.BasicGetAsync(queue: "transactionPool", autoAck: false);

                var transactionJson = Encoding.UTF8.GetString(result!.Body.ToArray());
                var transaction = JsonSerializer.Deserialize<Transaction>(transactionJson);
                transactionsList.Add(transaction!);

                mappedResult.Add(result, transaction!);
            }

            var selectedTransactions = GetBestTransactions(transactionsList, 1024);

            foreach (var result in mappedResult.Where(res => 
            IsTransactionInSelectedTransactions(res.Value, selectedTransactions)))
            {
                await channel.BasicRejectAsync(result.Key.DeliveryTag, false);
            }

            getResultQueue.Clear();
            mappedResult.Clear();

            return MapToRequest(selectedTransactions, currentBlock);
        }

        private List<ProccessTransactionRequest> MapToRequest(List<Transaction> transactions, int blockIndex)
        {
            List<ProccessTransactionRequest> request = new();

            foreach (var transaction in transactions)
            {
                request.Add(new ProccessTransactionRequest
                {
                    BlockIndex = blockIndex,
                    TransactionId = transaction.TransactionId
                });
            }
            return request;
        }

        private List<Transaction> GetBestTransactions(List<Transaction> transactions, int maxBytes)
        {
            int n = transactions.Count;
            int[,] dp = new int[n + 1, maxBytes + 1];

            // Build the DP table
            for (int i = 1; i <= n; i++)
            {
                for (int j = 0; j <= maxBytes; j++)
                {
                    if (transactions[i - 1].Bytes <= j)
                    {
                        dp[i, j] = Math.Max(
                            dp[i - 1, j],
                            dp[i - 1, j - transactions[i - 1].Bytes] + transactions[i - 1].Bytes
                        );
                    }
                    else
                    {
                        dp[i, j] = dp[i - 1, j];
                    }
                }
            }

            List<Transaction> selectedTransactions = new();
            int w = maxBytes;
            for (int i = n; i > 0 && w > 0; i--)
            {
                if (dp[i, w] != dp[i - 1, w])
                {
                    selectedTransactions.Add(transactions[i - 1]);
                    w -= transactions[i - 1].Bytes;
                }
            }

            return selectedTransactions;
        }

        private bool IsTransactionInSelectedTransactions(Transaction transaction, List<Transaction> selectedTransactions)
            => selectedTransactions.Contains(transaction);

    }
}
