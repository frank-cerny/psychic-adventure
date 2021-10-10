using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.Sqlite
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    DatePurchased = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ItemType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Cost = table.Column<double>(type: "REAL", nullable: true),
                    UsageCount = table.Column<int>(type: "INTEGER", nullable: true),
                    UnitCost = table.Column<double>(type: "REAL", nullable: true),
                    Units = table.Column<int>(type: "INTEGER", nullable: true),
                    CapitalItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    NonCapitalItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    RevenueItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    ProjectId = table.Column<int>(type: "INTEGER", nullable: true),
                    NonCapitalItem_UnitCost = table.Column<double>(type: "REAL", nullable: true),
                    NonCapitalItem_Units = table.Column<int>(type: "INTEGER", nullable: true),
                    SalePrice = table.Column<double>(type: "REAL", nullable: true),
                    PlatformSoldOn = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    RevenueItemType = table.Column<string>(type: "TEXT", nullable: true),
                    IsPending = table.Column<bool>(type: "INTEGER", nullable: true),
                    Weight = table.Column<double>(type: "REAL", nullable: true),
                    DateSold = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RevenueItem_ProjectId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Items_Items_CapitalItemId",
                        column: x => x.CapitalItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Items_NonCapitalItemId",
                        column: x => x.NonCapitalItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Items_RevenueItemId",
                        column: x => x.RevenueItemId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Items_Projects_RevenueItem_ProjectId",
                        column: x => x.RevenueItem_ProjectId,
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
                        name: "FK_CapitalItemProject_Items_CapitalItemsId",
                        column: x => x.CapitalItemsId,
                        principalTable: "Items",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CapitalItemProject_Projects_ProjectsId",
                        column: x => x.ProjectsId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                name: "IX_Items_CapitalItemId",
                table: "Items",
                column: "CapitalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name_DatePurchased",
                table: "Items",
                columns: new[] { "Name", "DatePurchased" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_NonCapitalItemId",
                table: "Items",
                column: "NonCapitalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_ProjectId",
                table: "Items",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_RevenueItem_ProjectId",
                table: "Items",
                column: "RevenueItem_ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Items_RevenueItemId",
                table: "Items",
                column: "RevenueItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Title",
                table: "Projects",
                column: "Title",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bikes");

            migrationBuilder.DropTable(
                name: "CapitalItemProject");

            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
