using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class edittanle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GenderType",
                table: "Products",
                newName: "Image");

            migrationBuilder.AddColumn<long>(
                name: "GoldPriceId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Factors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GoldPrice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PricePerGram = table.Column<double>(type: "float", nullable: false),
                    GenderType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoldPrice", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_GoldPriceId",
                table: "Products",
                column: "GoldPriceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_GoldPrice_GoldPriceId",
                table: "Products",
                column: "GoldPriceId",
                principalTable: "GoldPrice",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_GoldPrice_GoldPriceId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "GoldPrice");

            migrationBuilder.DropIndex(
                name: "IX_Products_GoldPriceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GoldPriceId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Factors");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Products",
                newName: "GenderType");
        }
    }
}
