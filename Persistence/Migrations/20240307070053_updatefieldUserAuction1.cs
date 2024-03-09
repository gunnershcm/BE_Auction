using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class updatefieldUserAuction1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuctions_Users_UserId",
                table: "UserAuctions");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserAuctions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuctions_Users_UserId",
                table: "UserAuctions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAuctions_Users_UserId",
                table: "UserAuctions");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "UserAuctions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAuctions_Users_UserId",
                table: "UserAuctions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
