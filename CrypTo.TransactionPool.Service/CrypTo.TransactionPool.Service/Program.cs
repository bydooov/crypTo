namespace CrypTo.TransactionPool.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddSingleton<TransactionPoolProcessor>();
            builder.Services.AddHostedService<TransactionPoolWorker>();
           
            var host = builder.Build();
            host.Run();
        }
    }
}