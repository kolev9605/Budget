using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Persistance.Migrations
{
    public partial class AddIsInitialColumnInCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInitial",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInitial",
                table: "Categories");
        }
    }
}
