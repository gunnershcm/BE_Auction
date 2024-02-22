using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updateArea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyDescription",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PropertyImage",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "PropertyTitle",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Properties",
                newName: "Ward");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Properties",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Properties",
                newName: "District");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Properties",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "Area",
                table: "Posts",
                newName: "PropertyArea");

            migrationBuilder.AddColumn<double>(
                name: "PropertyArea",
                table: "Properties",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyArea",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "Ward",
                table: "Properties",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "Properties",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "Properties",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Properties",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "PropertyArea",
                table: "Posts",
                newName: "Area");

            migrationBuilder.AddColumn<string>(
                name: "PropertyDescription",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyImage",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PropertyTitle",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
