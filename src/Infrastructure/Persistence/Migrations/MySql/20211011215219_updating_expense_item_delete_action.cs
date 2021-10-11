using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.MySql
{
    public partial class updating_expense_item_delete_action : Migration
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

            migrationBuilder.DropIndex(
                name: "IX_ExpenseItems_RevenueItemId",
                table: "ExpenseItems");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_CapitalItems_CapitalItemId",
                table: "ExpenseItems",
                column: "CapitalItemId",
                principalTable: "CapitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseItems_NonCapitalItems_NonCapitalItemId",
                table: "ExpenseItems",
                column: "NonCapitalItemId",
                principalTable: "NonCapitalItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_CapitalItems_CapitalItemId",
                table: "ExpenseItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseItems_NonCapitalItems_NonCapitalItemId",
                table: "ExpenseItems");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_RevenueItemId",
                table: "ExpenseItems",
                column: "RevenueItemId");

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
    }
}
