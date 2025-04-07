using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AurumPay.Application.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCheckoutSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_items_checkout_sessions_CheckoutSessionId",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "SessionToken",
                table: "checkout_sessions");

            migrationBuilder.DropColumn(
                name: "ProductName",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "cart_items");

            migrationBuilder.AlterColumn<Guid>(
                name: "CheckoutSessionId",
                table: "cart_items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_items_checkout_sessions_CheckoutSessionId",
                table: "cart_items",
                column: "CheckoutSessionId",
                principalTable: "checkout_sessions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cart_items_checkout_sessions_CheckoutSessionId",
                table: "cart_items");

            migrationBuilder.AddColumn<string>(
                name: "SessionToken",
                table: "checkout_sessions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "CheckoutSessionId",
                table: "cart_items",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<string>(
                name: "ProductName",
                table: "cart_items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "cart_items",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddForeignKey(
                name: "FK_cart_items_checkout_sessions_CheckoutSessionId",
                table: "cart_items",
                column: "CheckoutSessionId",
                principalTable: "checkout_sessions",
                principalColumn: "Id");
        }
    }
}
