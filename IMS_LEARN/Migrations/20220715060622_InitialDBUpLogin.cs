using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS_LEARN.Migrations
{
    public partial class InitialDBUpLogin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PassWord",
                table: "Staffs",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Staffs",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Staffs_UserName",
                table: "Staffs",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Staffs_UserName",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "PassWord",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Staffs");
        }
    }
}
