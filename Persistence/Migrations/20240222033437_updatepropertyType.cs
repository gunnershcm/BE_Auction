using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updatepropertyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccoountHolder",
                table: "Users",
                newName: "BankNameHolder");

            migrationBuilder.RenameColumn(
                name: "Response",
                table: "Posts",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "PropertyAddress",
                table: "Posts",
                newName: "PropertyWard");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "PropertyTypeId",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Area",
                table: "Posts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "PropertyCity",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyDistrict",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyStreet",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PropertyTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_DeletedAt",
                table: "Users",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_UserAuctions_DeletedAt",
                table: "UserAuctions",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionTypes_DeletedAt",
                table: "TransactionTypes",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_DeletedAt",
                table: "Transactions",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_DeletedAt",
                table: "Properties",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_DeletedAt",
                table: "Posts",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_DeletedAt",
                table: "Notifications",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Logs_DeletedAt",
                table: "Logs",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_DeletedAt",
                table: "Auctions",
                column: "DeletedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyTypes_DeletedAt",
                table: "PropertyTypes",
                column: "DeletedAt");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropTable(
                name: "PropertyTypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_DeletedAt",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserAuctions_DeletedAt",
                table: "UserAuctions");

            migrationBuilder.DropIndex(
                name: "IX_TransactionTypes_DeletedAt",
                table: "TransactionTypes");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_DeletedAt",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Properties_DeletedAt",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Posts_DeletedAt",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_DeletedAt",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Logs_DeletedAt",
                table: "Logs");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_DeletedAt",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Area",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PropertyCity",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PropertyDistrict",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PropertyStreet",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "BankNameHolder",
                table: "Users",
                newName: "AccoountHolder");

            migrationBuilder.RenameColumn(
                name: "Reason",
                table: "Posts",
                newName: "Response");

            migrationBuilder.RenameColumn(
                name: "PropertyWard",
                table: "Posts",
                newName: "PropertyAddress");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
