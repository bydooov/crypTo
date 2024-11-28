using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrypTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockTransactionReleashionship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "BlockchainTransactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BlockIndex",
                table: "BlockchainTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BlockchainTransactions_BlockIndex",
                table: "BlockchainTransactions",
                column: "BlockIndex");

            migrationBuilder.AddForeignKey(
                name: "FK_BlockchainTransactions_Blockchain_BlockIndex",
                table: "BlockchainTransactions",
                column: "BlockIndex",
                principalTable: "Blockchain",
                principalColumn: "Index",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlockchainTransactions_Blockchain_BlockIndex",
                table: "BlockchainTransactions");

            migrationBuilder.DropIndex(
                name: "IX_BlockchainTransactions_BlockIndex",
                table: "BlockchainTransactions");

            migrationBuilder.DropColumn(
                name: "BlockIndex",
                table: "BlockchainTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "Signature",
                table: "BlockchainTransactions",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256);
        }
    }
}
