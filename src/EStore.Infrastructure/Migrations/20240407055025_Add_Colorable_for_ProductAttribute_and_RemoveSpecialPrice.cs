using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Colorable_for_ProductAttribute_and_RemoveSpecialPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SpecialPriceEndDateTime",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SpecialPriceStartDateTime",
                table: "Products");

            migrationBuilder.AddColumn<bool>(
                name: "Colorable",
                table: "ProductAttributes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Colorable",
                table: "ProductAttributes");

            migrationBuilder.AddColumn<decimal>(
                name: "SpecialPrice",
                table: "Products",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SpecialPriceEndDateTime",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SpecialPriceStartDateTime",
                table: "Products",
                type: "datetime2",
                nullable: true);
        }
    }
}
