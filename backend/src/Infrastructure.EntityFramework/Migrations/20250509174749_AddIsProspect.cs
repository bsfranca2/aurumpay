using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AurumPay.Infrastructure.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class AddIsProspect : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProspect",
                table: "Customers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProspect",
                table: "Customers");
        }
    }
}
