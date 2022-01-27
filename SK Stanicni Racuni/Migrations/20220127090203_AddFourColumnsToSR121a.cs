using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SK_Stanicni_Racuni.Migrations
{
    public partial class AddFourColumnsToSR121a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlagajnikFR",
                table: "SR_K121a",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DatumVracanjaFR",
                table: "SR_K121a",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ObracunFR",
                table: "SR_K121a",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RedniBroj",
                table: "SR_K121a",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Saobracaj",
                table: "SR_K121a",
                type: "nvarchar(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlagajnikFR",
                table: "SR_K121a");

            migrationBuilder.DropColumn(
                name: "DatumVracanjaFR",
                table: "SR_K121a");

            migrationBuilder.DropColumn(
                name: "ObracunFR",
                table: "SR_K121a");

            migrationBuilder.DropColumn(
                name: "RedniBroj",
                table: "SR_K121a");

            migrationBuilder.DropColumn(
                name: "Saobracaj",
                table: "SR_K121a");
        }
    }
}
