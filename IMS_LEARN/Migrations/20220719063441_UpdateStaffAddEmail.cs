using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS_LEARN.Migrations
{
    public partial class UpdateStaffAddEmail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Staffs",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Staffs");
        }
    }
}
