
using CrypTo.Bussines.Services.Blockchain;
using CrypTo.Bussines.Services.Cache;
using CrypTo.Bussines.Services.Mine;
using CrypTo.Bussines.Services.RabbitMQ;
using CrypTo.Bussines.Services.Transactions;
using CrypTo.Bussines.Services.Wallets;
using CrypTo.Data.Repositories;
using CrypTo.Infrastructure.Context;
using CrypTo.Infrastructure.Repositories.Blockchain;
using CrypTo.Infrastructure.Repositories.Transactions;
using CrypTo.Infrastructure.Repositories.Wallets;
using CrypTo.Infrastructure.Services.Blockchain;
using CrypTo.Infrastructure.Services.Cache;
using CrypTo.Infrastructure.Services.Mine;
using CrypTo.Infrastructure.Services.RabbitMQ;
using CrypTo.Infrastructure.Services.Transactions;
using CrypTo.Infrastructure.Services.Wallets;
using Microsoft.EntityFrameworkCore;

namespace CrypTo.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddTransient<IWalletRepository, WalletRepository>();
            builder.Services.AddTransient<IWalletService, WalletService>();
            builder.Services.AddTransient<IMiningService, MiningService>();
            builder.Services.AddTransient<ITransactionRepository, TransactionRepository>();
            builder.Services.AddTransient<ITransactionService, TransactionService>();
            builder.Services.AddTransient<IRabbitMQService, RabbitMQService>();
            builder.Services.AddTransient<IBlockRepository, BlockRepository>();
            builder.Services.AddTransient<IBlockChainService, BlockChainService>();
            builder.Services.AddSingleton<ICacheService, CacheService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
