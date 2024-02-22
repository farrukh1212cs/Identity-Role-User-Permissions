using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Insurance.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddFatherNameInStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FatherName",
                table: "Student",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherName",
                table: "Student");
        }
    }
}
