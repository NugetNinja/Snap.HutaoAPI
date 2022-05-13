using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.HutaoAPI.Migrations
{
    public partial class AddUniKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnionId",
                table: "ReliquarySetDetails",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnionId",
                table: "ReliquarySetDetails");
        }
    }
}
