using Microsoft.EntityFrameworkCore.Migrations;

namespace SK_Stanicni_Racuni.Migrations
{
    public partial class AddSaobracajColumnToSR_161f : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Saobracaj",
                table: "SR_K161f",
                type: "nvarchar(1)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Saobracaj",
                table: "SR_K161f");
        }
    }
}
