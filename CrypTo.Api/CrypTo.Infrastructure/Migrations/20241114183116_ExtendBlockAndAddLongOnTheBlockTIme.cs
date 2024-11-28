using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrypTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendBlockAndAddLongOnTheBlockTIme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "BlockTime",
                table: "Blockchain",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BlockTime",
                table: "Blockchain",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
