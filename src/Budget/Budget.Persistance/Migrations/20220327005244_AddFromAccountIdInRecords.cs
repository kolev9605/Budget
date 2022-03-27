using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Persistance.Migrations
{
    public partial class AddFromAccountIdInRecords : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromAccountId",
                table: "Records",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Records_FromAccountId",
                table: "Records",
                column: "FromAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Accounts_FromAccountId",
                table: "Records",
                column: "FromAccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Accounts_FromAccountId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_FromAccountId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "FromAccountId",
                table: "Records");
        }
    }
}
