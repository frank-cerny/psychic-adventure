using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.MySql
{
    public partial class updating_fkeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_CapitalItems_CapitalItemId",
                table: "ExpenseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_NonCapitalItems_NonCapitalItemId",
                table: "ExpenseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_RevenueItems_RevenueItemId",
                table: "ExpenseItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_CapitalItems_CapitalItemId",
                table: "ExpenseItems",
                column: "CapitalItemId",
                principalTable: "CapitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_NonCapitalItems_NonCapitalItemId",
                table: "ExpenseItems",
                column: "NonCapitalItemId",
                principalTable: "NonCapitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_RevenueItems_RevenueItemId",
                table: "ExpenseItems",
                column: "RevenueItemId",
                principalTable: "RevenueItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_CapitalItems_CapitalItemId",
                table: "ExpenseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_NonCapitalItems_NonCapitalItemId",
                table: "ExpenseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_RevenueItems_RevenueItemId",
                table: "ExpenseItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_CapitalItems_CapitalItemId",
                table: "ExpenseItems",
                column: "CapitalItemId",
                principalTable: "CapitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_NonCapitalItems_NonCapitalItemId",
                table: "ExpenseItems",
                column: "NonCapitalItemId",
                principalTable: "NonCapitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_RevenueItems_RevenueItemId",
                table: "ExpenseItems",
                column: "RevenueItemId",
                principalTable: "RevenueItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
