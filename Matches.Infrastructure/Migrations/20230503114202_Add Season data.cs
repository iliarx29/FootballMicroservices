using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Matches.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSeasondata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Seasons",
                columns: new[] { "Id", "EndDate", "LeagueId", "StartDate", "TeamWinnerId", "Years" },
                values: new object[] { new Guid("e0f718f4-afcd-4d8e-aa56-5b210282163e"), null, new Guid("6e64b1e4-d662-4c04-8d67-0db65ead9eb7"), new DateTime(2022, 8, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "2022/2023" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Seasons",
                keyColumn: "Id",
                keyValue: new Guid("e0f718f4-afcd-4d8e-aa56-5b210282163e"));
        }
    }
}
