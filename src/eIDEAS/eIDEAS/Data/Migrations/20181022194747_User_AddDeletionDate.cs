using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eIDEAS.Data.Migrations
{
    public partial class User_AddDeletionDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime?>(
                name: "DateDeleted",
                table: "AspNetUsers",
                nullable: true,
                defaultValue: null);            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }

    }
}
