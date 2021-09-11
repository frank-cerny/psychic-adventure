using Microsoft.EntityFrameworkCore.Migrations;

namespace bike_selling_app.Infrastructure.Persistence.Migrations.Sqlite
{
    public partial class updatingprojectentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Projects",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Projects");
        }
    }
}
