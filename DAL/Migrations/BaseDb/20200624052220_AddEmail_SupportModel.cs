using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations.BaseDb
{
    public partial class AddEmail_SupportModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Tb_Supports",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Tb_Supports");
        }
    }
}
