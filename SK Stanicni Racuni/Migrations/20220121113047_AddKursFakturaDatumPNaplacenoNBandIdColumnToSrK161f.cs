using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SK_Stanicni_Racuni.Migrations
{
    public partial class AddKursFakturaDatumPNaplacenoNBandIdColumnToSrK161f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "SR_K161f",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "FakturaDatumP",
                table: "SR_K161f",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Kurs",
                table: "SR_K161f",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NaplacenoNB",
                table: "SR_K161f",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.AddColumn<decimal>(
            //    name: "K165a_iznos",
            //    table: "SlogKalk",
            //    type: "decimal(18,2)",
            //    nullable: false,
            //    defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SR_K161f",
                table: "SR_K161f",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SR_K161f",
                table: "SR_K161f");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "SR_K161f");

            migrationBuilder.DropColumn(
                name: "FakturaDatumP",
                table: "SR_K161f");

            migrationBuilder.DropColumn(
                name: "Kurs",
                table: "SR_K161f");

            migrationBuilder.DropColumn(
                name: "NaplacenoNB",
                table: "SR_K161f");

        //    migrationBuilder.DropColumn(
        //        name: "K165a_iznos",
        //        table: "SlogKalk");
        }
    }
}
