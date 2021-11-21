using Microsoft.EntityFrameworkCore.Migrations;

namespace Meetups.Providers.Migrations
{
    public partial class pk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MeetupUsers",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MeetupUsers",
                table: "MeetupUsers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MeetupUsers",
                table: "MeetupUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "MeetupUsers");
        }
    }
}
