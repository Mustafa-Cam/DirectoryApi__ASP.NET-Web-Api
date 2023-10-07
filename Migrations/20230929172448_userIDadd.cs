using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryApi.Migrations
{
    public partial class userIDadd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directory_Users_UserId",
                table: "Directory");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Directory",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Directory_Users_UserId",
                table: "Directory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Directory_Users_UserId",
                table: "Directory");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Directory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Directory_Users_UserId",
                table: "Directory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
