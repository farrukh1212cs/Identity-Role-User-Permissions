using Microsoft.EntityFrameworkCore.Migrations;

namespace Insurance.DataAccess.Migrations
{
    public partial class AddNewApiColumnInDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "API",
                table: "Menus",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "API",
                table: "Menus");
        }
    }
}
