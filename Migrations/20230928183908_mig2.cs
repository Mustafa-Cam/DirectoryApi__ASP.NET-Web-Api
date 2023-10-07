using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DirectoryApi.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Directory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Directory_UserId",
                table: "Directory",
                column: "UserId");

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

            migrationBuilder.DropIndex(
                name: "IX_Directory_UserId",
                table: "Directory");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Directory");
        }
    }
}
