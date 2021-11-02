using Microsoft.EntityFrameworkCore.Migrations;

namespace Providers.Migrations
{
    public partial class MeetupUser_Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetupUsers");

            migrationBuilder.CreateTable(
                name: "MeetupUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetupUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MeetupUser_Meetup_MeetupId",
                        column: x => x.MeetupId,
                        principalTable: "Meetup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MeetupUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetupUser_MeetupId",
                table: "MeetupUser",
                column: "MeetupId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetupUser_UserId",
                table: "MeetupUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetupUser");

            migrationBuilder.CreateTable(
                name: "MeetupUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeetupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetupUsers", x => x.Id);
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
                name: "IX_MeetupUsers_MeetupId",
                table: "MeetupUsers",
                column: "MeetupId");

            migrationBuilder.CreateIndex(
                name: "IX_MeetupUsers_UserId",
                table: "MeetupUsers",
                column: "UserId");
        }
    }
}
