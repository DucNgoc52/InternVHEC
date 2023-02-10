using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS_LEARN.Migrations
{
    public partial class InitialUDnote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Permits",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Permits");
        }
    }
}
