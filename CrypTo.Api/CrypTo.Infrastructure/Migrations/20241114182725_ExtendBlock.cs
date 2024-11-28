using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrypTo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ExtendBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BlockTime",
                table: "Blockchain",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Blockchain",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockTime",
                table: "Blockchain");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Blockchain");
        }
    }
}
