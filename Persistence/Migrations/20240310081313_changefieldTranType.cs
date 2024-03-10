using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class changefieldTranType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TransactionTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "TransactionTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "TransactionTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_DeletedAt",
                table: "TransactionTypes",
                column: "DeletedAt");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionTypes_DeletedAt",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "TransactionTypes");
        }
    }
}
