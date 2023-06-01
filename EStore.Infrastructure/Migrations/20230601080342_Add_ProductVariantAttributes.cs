using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infrastructure.Migrations
{
    public partial class Add_ProductVariantAttributes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductVariantAttributeCombinations",
                columns: table => new
                {
                    ProductVariantAttributeCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StockQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AssignedProductImageIds = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttributeCombinations", x => new { x.ProductVariantAttributeCombinationId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributeCombinations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantAttributes",
                columns: table => new
                {
                    ProductVariantAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttributes", x => new { x.ProductVariantAttributeId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributes_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantAttributeSelections",
                columns: table => new
                {
                    ProductVariantAttributeSelectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductVariantAttributeCombinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductVariantAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductVariantAttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttributeSelections", x => new { x.ProductVariantAttributeSelectionId, x.ProductVariantAttributeCombinationId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributeSelections_ProductVariantAttributeCombinations_ProductVariantAttributeCombinationId_ProductId",
                        columns: x => new { x.ProductVariantAttributeCombinationId, x.ProductId },
                        principalTable: "ProductVariantAttributeCombinations",
                        principalColumns: new[] { "ProductVariantAttributeCombinationId", "ProductId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductVariantAttributeValues",
                columns: table => new
                {
                    ProductVariantAttributeValueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductVariantAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductVariantAttributeValues", x => new { x.ProductVariantAttributeValueId, x.ProductVariantAttributeId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_ProductVariantAttributeValues_ProductVariantAttributes_ProductVariantAttributeId_ProductId",
                        columns: x => new { x.ProductVariantAttributeId, x.ProductId },
                        principalTable: "ProductVariantAttributes",
                        principalColumns: new[] { "ProductVariantAttributeId", "ProductId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributeCombinations_ProductId",
                table: "ProductVariantAttributeCombinations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributes_ProductId",
                table: "ProductVariantAttributes",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributeSelections_ProductVariantAttributeCombinationId_ProductId",
                table: "ProductVariantAttributeSelections",
                columns: new[] { "ProductVariantAttributeCombinationId", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariantAttributeValues_ProductVariantAttributeId_ProductId",
                table: "ProductVariantAttributeValues",
                columns: new[] { "ProductVariantAttributeId", "ProductId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductVariantAttributeSelections");

            migrationBuilder.DropTable(
                name: "ProductVariantAttributeValues");

            migrationBuilder.DropTable(
                name: "ProductVariantAttributeCombinations");

            migrationBuilder.DropTable(
                name: "ProductVariantAttributes");
        }
    }
}
