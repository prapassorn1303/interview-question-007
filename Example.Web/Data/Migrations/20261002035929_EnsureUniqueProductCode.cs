using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Example.Web.Data.Migrations
{
    /// <inheritdoc />
    public partial class EnsureUniqueProductCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ProductCodeItems_Code",
                table: "ProductCodeItems",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductCodeItems_Code",
                table: "ProductCodeItems");
        }
    }
}
