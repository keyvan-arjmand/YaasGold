using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoldShop.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Addimagetoproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageBanner",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageThumb",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InsertDate",
                table: "Products",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBanner",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImageThumb",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InsertDate",
                table: "Products");
        }
    }
}
