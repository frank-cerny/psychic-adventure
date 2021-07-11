using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Migrations
{
    public partial class test_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CapitalItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    cost = table.Column<double>(type: "REAL", nullable: false),
                    datePurchased = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapitalItems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    dateStarted = table.Column<DateTime>(type: "TEXT", nullable: false),
                    dateEnded = table.Column<DateTime>(type: "TEXT", nullable: false),
                    netValue = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    serialNumber = table.Column<string>(type: "TEXT", nullable: true),
                    make = table.Column<string>(type: "TEXT", nullable: true),
                    model = table.Column<string>(type: "TEXT", nullable: true),
                    purchasePrice = table.Column<float>(type: "REAL", nullable: false),
                    purchasedFrom = table.Column<string>(type: "TEXT", nullable: true),
                    projectId = table.Column<int>(type: "INTEGER", nullable: false),
                    datePurchased = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.id);
                    table.ForeignKey(
                        name: "FK_Bikes_Projects_projectId",
                        column: x => x.projectId,
                        principalTable: "Projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CapitalItemProject",
                columns: table => new
                {
                    capitalItemsid = table.Column<int>(type: "INTEGER", nullable: false),
                    projectsid = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapitalItemProject", x => new { x.capitalItemsid, x.projectsid });
                    table.ForeignKey(
                        name: "FK_CapitalItemProject_CapitalItems_capitalItemsid",
                        column: x => x.capitalItemsid,
                        principalTable: "CapitalItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapitalItemProject_Projects_projectsid",
                        column: x => x.projectsid,
                        principalTable: "Projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NonCapitalItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    projectid = table.Column<int>(type: "INTEGER", nullable: true),
                    cost = table.Column<double>(type: "REAL", nullable: false),
                    datePurchased = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonCapitalItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_NonCapitalItems_Projects_projectid",
                        column: x => x.projectid,
                        principalTable: "Projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RevenueItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    description = table.Column<string>(type: "TEXT", nullable: true),
                    salePrice = table.Column<double>(type: "REAL", nullable: false),
                    platformSoldOn = table.Column<string>(type: "TEXT", nullable: true),
                    itemType = table.Column<string>(type: "TEXT", nullable: true),
                    isPending = table.Column<bool>(type: "INTEGER", nullable: false),
                    weight = table.Column<double>(type: "REAL", nullable: false),
                    dateSold = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Projectid = table.Column<int>(type: "INTEGER", nullable: true),
                    datePurchased = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_RevenueItems_Projects_Projectid",
                        column: x => x.Projectid,
                        principalTable: "Projects",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_projectId",
                table: "Bikes",
                column: "projectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_serialNumber",
                table: "Bikes",
                column: "serialNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CapitalItemProject_projectsid",
                table: "CapitalItemProject",
                column: "projectsid");

            migrationBuilder.CreateIndex(
                name: "IX_NonCapitalItems_projectid",
                table: "NonCapitalItems",
                column: "projectid");

            migrationBuilder.CreateIndex(
                name: "IX_RevenueItems_Projectid",
                table: "RevenueItems",
                column: "Projectid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "CapitalItemProject");

            migrationBuilder.DropTable(
                name: "NonCapitalItems");

            migrationBuilder.DropTable(
                name: "RevenueItems");

            migrationBuilder.DropTable(
                name: "CapitalItems");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
