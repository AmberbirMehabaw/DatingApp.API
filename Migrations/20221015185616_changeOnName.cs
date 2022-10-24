using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.API.Migrations
{
    public partial class changeOnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Values",
                newName: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Values",
                newName: "Username");
        }
    }
}
