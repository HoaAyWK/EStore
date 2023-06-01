using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infrastructure.Migrations
{
    public partial class Add_ProductAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductAttributes",
                columns: table => new
                {
                    ProductAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributes", x => x.ProductAttributeId);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeOptionSets",
                columns: table => new
                {
                    ProductAttributeOptionSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeOptionSets", x => new { x.ProductAttributeOptionSetId, x.ProductAttributeId });
                    table.ForeignKey(
                        name: "FK_ProductAttributeOptionSets_ProductAttributes_ProductAttributeId",
                        column: x => x.ProductAttributeId,
                        principalTable: "ProductAttributes",
                        principalColumn: "ProductAttributeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductAttributeOptions",
                columns: table => new
                {
                    ProductAttributeOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAttributeOptionSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProductAttributeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    PriceAdjustment = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductAttributeOptions", x => new { x.ProductAttributeOptionId, x.ProductAttributeOptionSetId, x.ProductAttributeId });
                    table.ForeignKey(
                        name: "FK_ProductAttributeOptions_ProductAttributeOptionSets_ProductAttributeOptionSetId_ProductAttributeId",
                        columns: x => new { x.ProductAttributeOptionSetId, x.ProductAttributeId },
                        principalTable: "ProductAttributeOptionSets",
                        principalColumns: new[] { "ProductAttributeOptionSetId", "ProductAttributeId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptions_ProductAttributeOptionSetId_ProductAttributeId",
                table: "ProductAttributeOptions",
                columns: new[] { "ProductAttributeOptionSetId", "ProductAttributeId" });

            migrationBuilder.CreateIndex(
                name: "IX_ProductAttributeOptionSets_ProductAttributeId",
                table: "ProductAttributeOptionSets",
                column: "ProductAttributeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductAttributeOptions");

            migrationBuilder.DropTable(
                name: "ProductAttributeOptionSets");

            migrationBuilder.DropTable(
                name: "ProductAttributes");
        }
    }
}
