using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_SoftDeletable_And_Remove_Alias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alias",
                table: "ProductAttributes");

            migrationBuilder.RenameColumn(
                name: "Alias",
                table: "ProductAttributeValues",
                newName: "Color");

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Products",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductAttributeValues",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DisplayOrder",
                table: "ProductAttributes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Discounts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Discounts",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Brands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedOnUtc",
                table: "Brands",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers",
                column: "PhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_PhoneNumber",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductAttributeValues");

            migrationBuilder.DropColumn(
                name: "DisplayOrder",
                table: "ProductAttributes");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "DeletedOnUtc",
                table: "Brands");

            migrationBuilder.RenameColumn(
                name: "Color",
                table: "ProductAttributeValues",
                newName: "Alias");

            migrationBuilder.AddColumn<string>(
                name: "Alias",
                table: "ProductAttributes",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);
        }
    }
}
