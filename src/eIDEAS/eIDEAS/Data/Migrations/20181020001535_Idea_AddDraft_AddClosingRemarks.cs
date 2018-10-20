using Microsoft.EntityFrameworkCore.Migrations;

namespace eIDEAS.Data.Migrations
{
    public partial class Idea_AddDraft_AddClosingRemarks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                table: "Idea",
                nullable: false,
                defaultValue: false);
            migrationBuilder.AddColumn<string>(
                name: "ClosingRemarks",
                table: "Idea",
                nullable: true,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
