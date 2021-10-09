using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.Sqlite
{
    public partial class adding_expense_item : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Cost",
                table: "NonCapitalItems",
                newName: "UnitCost");

            migrationBuilder.CreateTable(
                name: "ExpenseItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitCost = table.Column<double>(type: "REAL", nullable: false),
                    Units = table.Column<int>(type: "INTEGER", nullable: false),
                    CapitalItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    NonCapitalItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    RevenueItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseItem_CapitalItems_CapitalItemId",
                        column: x => x.CapitalItemId,
                        principalTable: "CapitalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseItem_NonCapitalItems_NonCapitalItemId",
                        column: x => x.NonCapitalItemId,
                        principalTable: "NonCapitalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseItem_RevenueItems_RevenueItemId",
                        column: x => x.RevenueItemId,
                        principalTable: "RevenueItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItem_CapitalItemId",
                table: "ExpenseItem",
                column: "CapitalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItem_NonCapitalItemId",
                table: "ExpenseItem",
                column: "NonCapitalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItem_RevenueItemId",
                table: "ExpenseItem",
                column: "RevenueItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseItem");

            migrationBuilder.RenameColumn(
                name: "UnitCost",
                table: "NonCapitalItems",
                newName: "Cost");
        }
    }
}
