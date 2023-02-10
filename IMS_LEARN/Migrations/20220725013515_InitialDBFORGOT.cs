using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS_LEARN.Migrations
{
    public partial class InitialDBFORGOT : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordCode",
                table: "Staffs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordCode",
                table: "Staffs");
        }
    }
}
