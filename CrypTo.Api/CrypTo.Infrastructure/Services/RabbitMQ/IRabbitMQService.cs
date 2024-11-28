namespace CrypTo.Infrastructure.Services.RabbitMQ
{
    public interface IRabbitMQService
    {
        Task PublishMessageAsync(string queueName, string message);
        Task<int> GetMessagesCountAsync(string queueName);
    }
}
