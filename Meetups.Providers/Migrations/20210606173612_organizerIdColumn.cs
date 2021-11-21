using Microsoft.EntityFrameworkCore.Migrations;

namespace Meetups.Providers.Migrations
{
    public partial class organizerIdColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendees",
                table: "Meetup");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrganizerId",
                table: "Meetup",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "MeetupUsers",
                columns: table => new
                {
                    MeetupId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.ForeignKey(
                        name: "FK_MeetupUsers_Meetup_MeetupId",
                        column: x => x.MeetupId,
                        principalTable: "Meetup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetupUsers_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Meetup_OrganizerId",
                table: "Meetup",
                column: "OrganizerId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetupUsers_MeetupId",
                table: "MeetupUsers",
                column: "MeetupId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetupUsers_UserId",
                table: "MeetupUsers",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meetup_User_OrganizerId",
                table: "Meetup",
                column: "OrganizerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meetup_User_OrganizerId",
                table: "Meetup");

            migrationBuilder.DropTable(
                name: "MeetupUsers");

            migrationBuilder.DropIndex(
                name: "IX_Meetup_OrganizerId",
                table: "Meetup");

            migrationBuilder.DropColumn(
                name: "OrganizerId",
                table: "Meetup");

            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "User",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Attendees",
                table: "Meetup",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
