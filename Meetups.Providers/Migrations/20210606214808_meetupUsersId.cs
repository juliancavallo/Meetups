using Microsoft.EntityFrameworkCore.Migrations;

namespace Meetups.Providers.Migrations
{
    public partial class meetupUsersId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "MeetupUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "MeetupUsers");
        }
    }
}
