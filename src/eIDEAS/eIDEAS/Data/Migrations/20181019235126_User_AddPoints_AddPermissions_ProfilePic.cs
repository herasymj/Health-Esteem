using Microsoft.EntityFrameworkCore.Migrations;

namespace eIDEAS.Data.Migrations
{
    public partial class User_AddPoints_AddPermissions_ProfilePic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdeaPoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<int>(
                name: "ParticipationPoints",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<int>(
                name: "Permissions",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "AspNetUsers",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
