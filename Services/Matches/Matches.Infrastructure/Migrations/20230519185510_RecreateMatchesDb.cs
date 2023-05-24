using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matches.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecreateMatchesDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Seasons_SeasonId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Matches_SeasonId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "SeasonId",
                table: "Matches",
                newName: "CompetitionId");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Matches",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Matchday",
                table: "Matches",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Season",
                table: "Matches",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "Matches",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Matchday",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Season",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Matches");

            migrationBuilder.RenameColumn(
                name: "CompetitionId",
                table: "Matches",
                newName: "SeasonId");

            migrationBuilder.AddColumn<Guid>(
                name: "LeagueId",
                table: "Matches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "Matches",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LeagueId = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TeamWinnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Years = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "EndDate", "LeagueId", "StartDate", "TeamWinnerId", "Years" },
                values: new object[] { new Guid("6de7e6c5-d265-4cbe-b81b-12b42b5737fb"), null, new Guid("6e64b1e4-d662-4c04-8d67-0db65ead9eb7"), new DateTime(2022, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "2022/2023" });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SeasonId",
                table: "Matches",
                column: "SeasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Seasons_SeasonId",
                table: "Matches",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
