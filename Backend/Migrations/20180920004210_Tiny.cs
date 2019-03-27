using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class Tiny : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Seasons_SeasonKey",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SeasonKey",
                table: "Users",
                newName: "SeasonId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SeasonKey",
                table: "Users",
                newName: "IX_Users_SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Seasons_SeasonId",
                table: "Users",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Seasons_SeasonId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "SeasonId",
                table: "Users",
                newName: "SeasonKey");

            migrationBuilder.RenameIndex(
                name: "IX_Users_SeasonId",
                table: "Users",
                newName: "IX_Users_SeasonKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Seasons_SeasonKey",
                table: "Users",
                column: "SeasonKey",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
