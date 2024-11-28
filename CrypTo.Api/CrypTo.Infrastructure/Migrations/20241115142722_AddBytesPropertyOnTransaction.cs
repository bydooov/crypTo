using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrypTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBytesPropertyOnTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Bytes",
                table: "BlockchainTransactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bytes",
                table: "BlockchainTransactions");
        }
    }
}
