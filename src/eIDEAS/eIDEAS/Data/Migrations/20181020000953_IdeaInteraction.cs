using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace eIDEAS.Data.Migrations
{
    public partial class IdeaInteraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "IdeaInteraction",
               columns: table => new
               {
                   ID = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                   UserID = table.Column<Guid>(nullable: false),
                   IdeaID = table.Column<int>(nullable: false),
                   IsTracked = table.Column<bool>(nullable: true),
                   Rating = table.Column<int>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_IdeaInteraction", x => x.ID);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
