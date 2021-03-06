using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.Sqlite
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CapitalItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Cost = table.Column<double>(type: "REAL", nullable: false),
                    UsageCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapitalItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    DateStarted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DateEnded = table.Column<DateTime>(type: "TEXT", nullable: true),
                    NetValue = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Make = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PurchasePrice = table.Column<double>(type: "REAL", nullable: false),
                    PurchasedFrom = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bikes_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CapitalItemProject",
                columns: table => new
                {
                    CapitalItemsId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjectsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapitalItemProject", x => new { x.CapitalItemsId, x.ProjectsId });
                    table.ForeignKey(
                        name: "FK_CapitalItemProject_CapitalItems_CapitalItemsId",
                        column: x => x.CapitalItemsId,
                        principalTable: "CapitalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapitalItemProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NonCapitalItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    UnitCost = table.Column<double>(type: "REAL", nullable: false),
                    Units = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NonCapitalItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NonCapitalItems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RevenueItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SalePrice = table.Column<double>(type: "REAL", nullable: false),
                    PlatformSoldOn = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RevenueItemType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsPending = table.Column<bool>(type: "INTEGER", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    DateSold = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevenueItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RevenueItems_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UnitCost = table.Column<double>(type: "REAL", nullable: false),
                    Units = table.Column<int>(type: "INTEGER", nullable: false),
                    CapitalItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    NonCapitalItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    RevenueItemId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_CapitalItems_CapitalItemId",
                        column: x => x.CapitalItemId,
                        principalTable: "CapitalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_NonCapitalItems_NonCapitalItemId",
                        column: x => x.NonCapitalItemId,
                        principalTable: "NonCapitalItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseItems_RevenueItems_RevenueItemId",
                        column: x => x.RevenueItemId,
                        principalTable: "RevenueItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_ProjectId",
                table: "Bikes",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Bikes_SerialNumber_Make_Model",
                table: "Bikes",
                columns: new[] { "SerialNumber", "Make", "Model" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CapitalItemProject_ProjectsId",
                table: "CapitalItemProject",
                column: "ProjectsId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_CapitalItemId",
                table: "ExpenseItems",
                column: "CapitalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_Name_DatePurchased",
                table: "ExpenseItems",
                columns: new[] { "Name", "DatePurchased" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_NonCapitalItemId",
                table: "ExpenseItems",
                column: "NonCapitalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseItems_RevenueItemId",
                table: "ExpenseItems",
                column: "RevenueItemId");

            migrationBuilder.CreateIndex(
                name: "IX_NonCapitalItems_ProjectId",
                table: "NonCapitalItems",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Title",
                table: "Projects",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RevenueItems_ProjectId",
                table: "RevenueItems",
                column: "ProjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "CapitalItemProject");

            migrationBuilder.DropTable(
                name: "ExpenseItems");

            migrationBuilder.DropTable(
                name: "CapitalItems");

            migrationBuilder.DropTable(
                name: "NonCapitalItems");

            migrationBuilder.DropTable(
                name: "RevenueItems");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
