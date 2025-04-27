using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AurumPay.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddProductPublicId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_StoreId",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "Products",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

#pragma warning disable CA1861
            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId_PublicId",
                table: "Products",
                columns: new[] { "StoreId", "PublicId" },
                unique: true);
#pragma warning restore CA1861
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_StoreId_PublicId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StoreId",
                table: "Products",
                column: "StoreId");
        }
    }
}
