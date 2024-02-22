using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class propertyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId",
                table: "Properties");

            migrationBuilder.AddColumn<int>(
                name: "PropertyTypeId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PropertyTypeId",
                table: "Posts",
                column: "PropertyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PropertyTypes_PropertyTypeId",
                table: "Posts",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PropertyTypes_PropertyTypeId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PropertyTypeId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PropertyTypeId",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "PropertyTypeId",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyTypes_PropertyTypeId",
                table: "Properties",
                column: "PropertyTypeId",
                principalTable: "PropertyTypes",
                principalColumn: "Id");
        }
    }
}
