using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrypTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blockchain",
                columns: table => new
                {
                    Index = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    PreviousHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CurrentHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nonce = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blockchain", x => x.Index);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletAddress);
                });

            migrationBuilder.CreateTable(
                name: "BlockchainTransactions",
                columns: table => new
                {
                    TransactionId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SenderAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ReceiverAddress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Signature = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlockchainTransactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_BlockchainTransactions_Wallets_ReceiverAddress",
                        column: x => x.ReceiverAddress,
                        principalTable: "Wallets",
                        principalColumn: "WalletAddress",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BlockchainTransactions_Wallets_SenderAddress",
                        column: x => x.SenderAddress,
                        principalTable: "Wallets",
                        principalColumn: "WalletAddress",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainTransactions_ReceiverAddress",
                table: "BlockchainTransactions",
                column: "ReceiverAddress");

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainTransactions_SenderAddress",
                table: "BlockchainTransactions",
                column: "SenderAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blockchain");

            migrationBuilder.DropTable(
                name: "BlockchainTransactions");

            migrationBuilder.DropTable(
                name: "Wallets");
        }
    }
}
