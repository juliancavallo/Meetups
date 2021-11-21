using Microsoft.EntityFrameworkCore.Migrations;

namespace Meetups.Providers.Migrations
{
    public partial class FirstName_Col_Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "User",
                newName: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "User",
                newName: "Name");
        }
    }
}
