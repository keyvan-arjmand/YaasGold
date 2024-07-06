using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class addtypetogoldprice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PriceType",
                table: "GoldPrice",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceType",
                table: "GoldPrice");
        }
    }
}
