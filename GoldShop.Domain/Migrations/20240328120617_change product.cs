using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class changeproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSpecial",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<double>(
                name: "GoldPerDay",
                table: "Factors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSpecial",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GoldPerDay",
                table: "Factors");
        }
    }
}
