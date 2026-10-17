using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dingler.Data.Migrations.HexGame
{
    /// <inheritdoc />
    public partial class SeedTournamentState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "TournamentStates",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "WAITING" },
                    { 2, "RUNNING" },
                    { 3, "FINISHED" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TournamentStates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TournamentStates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TournamentStates",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
