using eIDEAS.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eIDEAS.Data.Migrations
{
    public partial class Point : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Point",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ActionID = table.Column<int>(nullable: false),
                    Value = table.Column<float>(nullable: false),
                    Type = table.Column<PointTypeEnum>(nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Point", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
