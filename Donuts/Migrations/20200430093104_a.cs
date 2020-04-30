using Microsoft.EntityFrameworkCore.Migrations;

namespace Donuts.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_UserName",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "User",
                newName: "CustomerName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User",
                newName: "CustomerId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Domain",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_CustomerName",
                table: "User",
                column: "CustomerName",
                unique: true,
                filter: "[CustomerName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_User_CustomerName",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "CustomerName",
                table: "User",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "CustomerId",
                table: "User",
                newName: "UserId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Domain",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }
    }
}
