using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matches.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Seasons",
                keyColumn: "Id",
                keyValue: new Guid("e0f718f4-afcd-4d8e-aa56-5b210282163e"));

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CountryName = table.Column<string>(type: "text", nullable: false),
                    TeamId = table.Column<Guid>(type: "uuid", nullable: true),
                    ShirtNumber = table.Column<int>(type: "integer", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Position = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AwayPlayersMatches",
                columns: table => new
                {
                    AwayMatchesId = table.Column<Guid>(type: "uuid", nullable: false),
                    AwayPlayersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AwayPlayersMatches", x => new { x.AwayMatchesId, x.AwayPlayersId });
                    table.ForeignKey(
                        name: "FK_AwayPlayersMatches_Matches_AwayMatchesId",
                        column: x => x.AwayMatchesId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AwayPlayersMatches_Players_AwayPlayersId",
                        column: x => x.AwayPlayersId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HomePlayersMatches",
                columns: table => new
                {
                    HomeMatchesId = table.Column<Guid>(type: "uuid", nullable: false),
                    HomePlayersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomePlayersMatches", x => new { x.HomeMatchesId, x.HomePlayersId });
                    table.ForeignKey(
                        name: "FK_HomePlayersMatches_Matches_HomeMatchesId",
                        column: x => x.HomeMatchesId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HomePlayersMatches_Players_HomePlayersId",
                        column: x => x.HomePlayersId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "EndDate", "LeagueId", "StartDate", "TeamWinnerId", "Years" },
                values: new object[] { new Guid("6de7e6c5-d265-4cbe-b81b-12b42b5737fb"), null, new Guid("6e64b1e4-d662-4c04-8d67-0db65ead9eb7"), new DateTime(2022, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "2022/2023" });

            migrationBuilder.CreateIndex(
                name: "IX_AwayPlayersMatches_AwayPlayersId",
                table: "AwayPlayersMatches",
                column: "AwayPlayersId");

            migrationBuilder.CreateIndex(
                name: "IX_HomePlayersMatches_HomePlayersId",
                table: "HomePlayersMatches",
                column: "HomePlayersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AwayPlayersMatches");

            migrationBuilder.DropTable(
                name: "HomePlayersMatches");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DeleteData(
                table: "Seasons",
                keyColumn: "Id",
                keyValue: new Guid("6de7e6c5-d265-4cbe-b81b-12b42b5737fb"));

            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "EndDate", "LeagueId", "StartDate", "TeamWinnerId", "Years" },
                values: new object[] { new Guid("e0f718f4-afcd-4d8e-aa56-5b210282163e"), null, new Guid("6e64b1e4-d662-4c04-8d67-0db65ead9eb7"), new DateTime(2022, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "2022/2023" });
        }
    }
}
