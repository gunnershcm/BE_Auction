using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class fixFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_Properties_PropertyId",
                table: "Auctions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }
    }
}
