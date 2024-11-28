
using CrypTo.TransactionPool.Service.Contracts;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace CrypTo.TransactionPool.Service
{
    public class TransactionPoolWorker : BackgroundService
    {
        private readonly ConnectionFactory _factory;
        private IConnection? _connection;
        private IChannel? _channel;
        private readonly TransactionPoolProcessor _processor;
        private readonly HttpClient _httpClient;

        public TransactionPoolWorker(TransactionPoolProcessor processor)
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
            _factory.AutomaticRecoveryEnabled = true;
            _factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

            _processor = processor;

            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://localhost:7034")
            };
        }

        private async Task EnsureConnectionAsync()
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _connection = await _factory.CreateConnectionAsync();
                _channel = await _connection.CreateChannelAsync();
                await _channel.BasicQosAsync(0, 1, false);
            }
            else if (_channel == null || !_channel.IsOpen)
            {
                _channel = await _connection.CreateChannelAsync();
                await _channel.BasicQosAsync(0, 1, false);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await EnsureConnectionAsync();

                Console.WriteLine("Debug1");

                await _channel.QueueDeclareAsync(
                    queue: "blockPool",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var consumer = new AsyncEventingBasicConsumer(_channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    var block = JsonSerializer.Deserialize<Block>(message);

                    try
                    {
                        var request = await _processor.ProcessTransactionsAsync(block!.Index);

                        await ProcessTransactionAsync(request).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.ToString()}");
                    }

                    Console.WriteLine($"[x] Received block notification: {message}");
                };

                while (!stoppingToken.IsCancellationRequested)
                {
                    await _channel.BasicConsumeAsync(queue: "blockPool", autoAck: true, consumer: consumer);
                    await Task.Delay(1000, stoppingToken); // Just to check the cancellation token periodically
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.ToString()}");
            }
            finally
            {
                // Ensure to close the channel and connection when the service stops
                await _channel?.CloseAsync()!;
                await _connection?.CloseAsync()!;
            }
        }

        private async Task ProcessTransactionAsync(List<ProccessTransactionRequest> transactions)
        {
            try
            {
                var url = "api/transactions/process";


                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = new StringContent(JsonSerializer.Serialize(transactions), Encoding.UTF8, "application/json")
                };

                var response = await _httpClient.SendAsync(request);
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
