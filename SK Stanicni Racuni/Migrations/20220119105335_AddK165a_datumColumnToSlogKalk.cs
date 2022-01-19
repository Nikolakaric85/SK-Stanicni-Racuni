using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SK_Stanicni_Racuni.Migrations
{
    public partial class AddK165a_datumColumnToSlogKalk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "K165a_datum",
                table: "SlogKalk",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "K165a_datum",
                table: "SlogKalk");
        }
    }
}
