using Microsoft.EntityFrameworkCore.Migrations;

namespace SK_Stanicni_Racuni.Migrations
{
    public partial class AddK165aColumnToSlogKalk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "K165a",
                table: "SlogKalk",
                type: "nvarchar(1)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "K165a",
                table: "SlogKalk");
        }
    }
}
