using CrypTo.Infrastructure.Services.RabbitMQ;
using RabbitMQ.Client;
using System.Text;

namespace CrypTo.Bussines.Services.RabbitMQ
{
    public class RabbitMQService : IRabbitMQService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMQService()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };
        }

        public async Task<int> GetMessagesCountAsync(string queueName)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            var result = await channel.QueueDeclarePassiveAsync(queueName);

            return (int)result.MessageCount;
        }

        public async Task PublishMessageAsync(string queueName, string message)
        {
            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();

            // Declare a queue (idempotent; will only be created if it doesn't exist)
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            // Publish the message
            await channel.BasicPublishAsync(
                exchange: "",
                routingKey: queueName,
                body: body);
        }
    }
}
