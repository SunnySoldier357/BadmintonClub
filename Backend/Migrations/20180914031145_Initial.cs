using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    GamesDrawn = table.Column<int>(nullable: false, defaultValue: 0),
                    GamesLost = table.Column<int>(nullable: false, defaultValue: 0),
                    GamesWon = table.Column<int>(nullable: false, defaultValue: 0),
                    PointsAgainst = table.Column<int>(nullable: false, defaultValue: 0),
                    PointsFor = table.Column<int>(nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SeasonNumber = table.Column<int>(nullable: false),
                    StatisticsId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seasons_Statistics_StatisticsId",
                        column: x => x.StatisticsId,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ClearanceLevel = table.Column<int>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    SeasonKey = table.Column<Guid>(nullable: false),
                    StatisticsId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true, defaultValue: "Member")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Seasons_SeasonKey",
                        column: x => x.SeasonKey,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Statistics_StatisticsId",
                        column: x => x.StatisticsId,
                        principalTable: "Statistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    DatePublished = table.Column<DateTime>(nullable: false),
                    Id = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),
                    PublisherId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlogPosts_Users_PublisherId",
                        column: x => x.PublisherId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OpponentScore = table.Column<int>(nullable: false),
                    PlayerScore = table.Column<int>(nullable: false),
                    OpponentId = table.Column<Guid>(nullable: false),
                    PlayerId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    SeasonId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Users_OpponentId",
                        column: x => x.OpponentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Users_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PublisherId",
                table: "BlogPosts",
                column: "PublisherId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_OpponentId",
                table: "Matches",
                column: "OpponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_PlayerId",
                table: "Matches",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SeasonId",
                table: "Matches",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_UserId",
                table: "Matches",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_StatisticsId",
                table: "Seasons",
                column: "StatisticsId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_SeasonKey",
                table: "Users",
                column: "SeasonKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatisticsId",
                table: "Users",
                column: "StatisticsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropTable(
                name: "Statistics");
        }
    }
}