using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "Auctions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "AuctionStatus",
                table: "Auctions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "AuctionStatus",
                table: "Auctions");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "Auctions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
