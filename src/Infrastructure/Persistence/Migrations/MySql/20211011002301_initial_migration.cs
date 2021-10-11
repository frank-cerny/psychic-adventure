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
                name: "CapitalItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePurchased = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Cost = table.Column<double>(type: "double", nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CapitalItems", x => x.Id);
                })
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "NonCapitalItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePurchased = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true),
                    UnitCost = table.Column<double>(type: "double", nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RevenueItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePurchased = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SalePrice = table.Column<double>(type: "double", nullable: false),
                    PlatformSoldOn = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RevenueItemType = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsPending = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Weight = table.Column<double>(type: "double", nullable: false),
                    DateSold = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: true)
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
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ExpenseItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(250)", maxLength: 250, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DatePurchased = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UnitCost = table.Column<double>(type: "double", nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    CapitalItemId = table.Column<int>(type: "int", nullable: true),
                    NonCapitalItemId = table.Column<int>(type: "int", nullable: true),
                    RevenueItemId = table.Column<int>(type: "int", nullable: true)
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
