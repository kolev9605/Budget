using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Persistance.Migrations
{
    public partial class AddCurrenciesAndPaymentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Records",
                newName: "Note");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "Records",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Records",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PaymentTypeId",
                table: "Records",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_CurrencyId",
                table: "Records",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Records_PaymentTypeId",
                table: "Records",
                column: "PaymentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Records_Currencies_CurrencyId",
                table: "Records",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Records_PaymentTypes_PaymentTypeId",
                table: "Records",
                column: "PaymentTypeId",
                principalTable: "PaymentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Records_Currencies_CurrencyId",
                table: "Records");

            migrationBuilder.DropForeignKey(
                name: "FK_Records_PaymentTypes_PaymentTypeId",
                table: "Records");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropIndex(
                name: "IX_Records_CurrencyId",
                table: "Records");

            migrationBuilder.DropIndex(
                name: "IX_Records_PaymentTypeId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Records");

            migrationBuilder.DropColumn(
                name: "PaymentTypeId",
                table: "Records");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Records",
                newName: "Description");
        }
    }
}
