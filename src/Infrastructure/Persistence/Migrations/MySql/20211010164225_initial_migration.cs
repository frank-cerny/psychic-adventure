using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.MySql
{
    public partial class initial_migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Title = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DateStarted = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DateEnded = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    NetValue = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Bikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SerialNumber = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Make = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Model = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PurchasePrice = table.Column<double>(type: "double", nullable: false),
                    PurchasedFrom = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    DatePurchased = table.Column<DateTime>(type: "datetime(6)", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePurchased = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ItemType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cost = table.Column<double>(type: "double", nullable: true),
                    UsageCount = table.Column<int>(type: "int", nullable: true),
                    UnitCost = table.Column<double>(type: "double", nullable: true),
                    Units = table.Column<int>(type: "int", nullable: true),
                    CapitalItemId = table.Column<int>(type: "int", nullable: true),
                    NonCapitalItemId = table.Column<int>(type: "int", nullable: true),
                    RevenueItemId = table.Column<int>(type: "int", nullable: true),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    NonCapitalItem_UnitCost = table.Column<double>(type: "double", nullable: true),
                    NonCapitalItem_Units = table.Column<int>(type: "int", nullable: true),
                    SalePrice = table.Column<double>(type: "double", nullable: true),
                    PlatformSoldOn = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RevenueItemType = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPending = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    Weight = table.Column<double>(type: "double", nullable: true),
                    DateSold = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RevenueItem_ProjectId = table.Column<int>(type: "int", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CapitalItemProject",
                columns: table => new
                {
                    CapitalItemsId = table.Column<int>(type: "int", nullable: false),
                    ProjectsId = table.Column<int>(type: "int", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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
