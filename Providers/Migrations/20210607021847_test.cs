using Microsoft.EntityFrameworkCore.Migrations;

namespace Providers.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MeetupUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MeetupUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
