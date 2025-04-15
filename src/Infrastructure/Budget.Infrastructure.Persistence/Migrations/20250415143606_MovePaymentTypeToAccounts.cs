using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Budget.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MovePaymentTypeToAccounts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_records_payment_types_payment_type_id",
                table: "records");

            migrationBuilder.DropIndex(
                name: "ix_records_payment_type_id",
                table: "records");

            migrationBuilder.DropColumn(
                name: "payment_type_id",
                table: "records");

            migrationBuilder.AddColumn<Guid>(
                name: "payment_type_id",
                table: "accounts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_accounts_payment_type_id",
                table: "accounts",
                column: "payment_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_accounts_payment_types_payment_type_id",
                table: "accounts",
                column: "payment_type_id",
                principalTable: "payment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_accounts_payment_types_payment_type_id",
                table: "accounts");

            migrationBuilder.DropIndex(
                name: "ix_accounts_payment_type_id",
                table: "accounts");

            migrationBuilder.DropColumn(
                name: "payment_type_id",
                table: "accounts");

            migrationBuilder.AddColumn<Guid>(
                name: "payment_type_id",
                table: "records",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_records_payment_type_id",
                table: "records",
                column: "payment_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_records_payment_types_payment_type_id",
                table: "records",
                column: "payment_type_id",
                principalTable: "payment_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
