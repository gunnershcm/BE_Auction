using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TransactionTypes_DeletedAt",
                table: "TransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_PropertyTypes_DeletedAt",
                table: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "TransactionTypes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "PropertyTypes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "PropertyTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "PropertyTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "PropertyTypes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_DeletedAt",
                table: "TransactionTypes",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_DeletedAt",
                table: "PropertyTypes",
                column: "DeletedAt");
        }
    }
}
