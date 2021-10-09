using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.MySql
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
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UnitCost = table.Column<double>(type: "double", nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    CapitalItemId = table.Column<int>(type: "int", nullable: true),
                    NonCapitalItemId = table.Column<int>(type: "int", nullable: true),
                    RevenueItemId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePurchased = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
