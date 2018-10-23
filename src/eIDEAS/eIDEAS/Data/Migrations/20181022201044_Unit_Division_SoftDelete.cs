using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eIDEAS.Data.Migrations
{
    public partial class Unit_Division_SoftDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime?>(
                name: "DateDeleted",
                table: "Unit",
                nullable: true,
                defaultValue: null);

            migrationBuilder.AddColumn<DateTime?>(
                name: "DateDeleted",
                table: "Division",
                nullable: true,
                defaultValue: null);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
