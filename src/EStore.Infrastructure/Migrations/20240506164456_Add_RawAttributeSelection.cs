using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_RawAttributeSelection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RawAttributeSelection",
                table: "ProductReviews",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RawAttributeSelection",
                table: "ProductReviews");
        }
    }
}
