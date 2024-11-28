using CrypTo.Infrastructure.Entities.Blockchain;
using CrypTo.Infrastructure.Entities.Transactions;
using CrypTo.Infrastructure.Entities.Wallets;
using Microsoft.EntityFrameworkCore;

namespace CrypTo.Infrastructure.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Block> Blockchain { get; set; }
        public DbSet<Transaction> BlockchainTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Wallet>(entity =>
            {
                entity.HasKey(e => e.WalletAddress);

                entity.Property(w => w.WalletAddress)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(w => w.PublicKey)
                    .HasMaxLength(255)
                    .IsRequired();

                entity.Property(w => w.PrivateKey)
                    .IsRequired();

                entity.Property(w => w.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(w => w.Balance)
                    .IsRequired()
                    .HasColumnType("decimal(18, 2)");

                entity.Property(w => w.IsDeleted)
                   .IsRequired();
            });

            modelBuilder.Entity<Block>(entity =>
            {
                entity.HasKey(e => e.Index);

                entity.Property(w => w.Index)
                    .ValueGeneratedNever()
                    .IsRequired();

                entity.Property(w => w.PreviousHash)
                    .IsRequired();

                entity.Property(w => w.CurrentHash)
                    .IsRequired();

                entity.Property(w => w.Timestamp)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(w => w.Nonce)
                   .IsRequired();
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.TransactionId);

                entity.Property(t => t.TransactionId)
                    .IsRequired();

                entity.Property(t => t.SenderAddress)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(t => t.ReceiverAddress)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(t => t.Amount)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(t => t.Fee)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(t => t.Timestamp)
                    .IsRequired();

                entity.Property(t => t.Signature)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(t => t.Signature)
                    .HasMaxLength(256);

                entity.HasOne(t => t.Sender)
                    .WithMany(w => w.Transactions)
                    .HasForeignKey(t => t.SenderAddress)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Block)
                   .WithMany(w => w.Transactions)
                   .HasForeignKey(t => t.BlockIndex)
                   .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(t => t.Receiver)
                    .WithMany()
                    .HasForeignKey(t => t.ReceiverAddress)
                    .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
