using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Persistance.Migrations
{
    public partial class AddRecordType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RecordType",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecordType",
                table: "Records");
        }
    }
}
