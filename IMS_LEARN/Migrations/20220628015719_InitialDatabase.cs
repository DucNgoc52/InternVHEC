using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS_LEARN.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PrjCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    PrjName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    RqDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Custom = table.Column<string>(type: "text", nullable: false),
                    Deparment = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Paymonth = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: true),
                    Remark = table.Column<string>(type: "text", nullable: true),
                    Update = table.Column<string>(type: "text", nullable: true),
                    UserUpdate = table.Column<string>(type: "text", nullable: true),
                    CreatDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");
        }
    }
}
