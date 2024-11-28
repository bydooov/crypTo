﻿// <auto-generated />
using System;
using CrypTo.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CrypTo.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CrypTo.Infrastructure.Entities.Blockchain.Block", b =>
                {
                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<long>("BlockTime")
                        .HasColumnType("bigint");

                    b.Property<string>("CurrentHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<int>("Nonce")
                        .HasColumnType("int");

                    b.Property<string>("PreviousHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.HasKey("Index");

                    b.ToTable("Blockchain");
                });

            modelBuilder.Entity("CrypTo.Infrastructure.Entities.Transactions.Transaction", b =>
                {
                    b.Property<string>("TransactionId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("BlockIndex")
                        .HasColumnType("int");

                    b.Property<int>("Bytes")
                        .HasColumnType("int");

                    b.Property<decimal>("Fee")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("ReceiverAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SenderAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Signature")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.HasKey("TransactionId");

                    b.HasIndex("BlockIndex");

                    b.HasIndex("ReceiverAddress");

                    b.HasIndex("SenderAddress");

                    b.ToTable("BlockchainTransactions");
                });

            modelBuilder.Entity("CrypTo.Infrastructure.Entities.Wallets.Wallet", b =>
                {
                    b.Property<string>("WalletAddress")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("PrivateKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PublicKey")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("WalletAddress");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("CrypTo.Infrastructure.Entities.Transactions.Transaction", b =>
                {
                    b.HasOne("CrypTo.Infrastructure.Entities.Blockchain.Block", "Block")
                        .WithMany("Transactions")
                        .HasForeignKey("BlockIndex")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CrypTo.Infrastructure.Entities.Wallets.Wallet", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverAddress")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("CrypTo.Infrastructure.Entities.Wallets.Wallet", "Sender")
                        .WithMany("Transactions")
                        .HasForeignKey("SenderAddress")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Block");

                    b.Navigation("Receiver");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("CrypTo.Infrastructure.Entities.Blockchain.Block", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("CrypTo.Infrastructure.Entities.Wallets.Wallet", b =>
                {
                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}