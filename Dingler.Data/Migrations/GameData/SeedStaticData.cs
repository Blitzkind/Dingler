using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dingler.Data.Migrations.GameData
{
        public partial class SeedStaticData : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FriendStatus",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Accepted" },
                    { 3, "Blocked" }
                });

            migrationBuilder.InsertData(
                table: "MatchType",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "SingleElimination" },
                    { 2, "Swiss" },
                    { 3, "SwissWithTop8" }
                });

            migrationBuilder.InsertData(
                table: "Rank",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Bronze" },
                    { 2, "Silver" },
                    { 3, "Gold" },
                    { 4, "Platinum" },
                    { 5, "Cosmic" }
                });

            migrationBuilder.InsertData(
                table: "Set",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "ShardsOfFate" },
                    { 2, "ShatteredDestiny" },
                    { 3, "ArmiesOfMyth" },
                    { 4, "PrimalDawn" },
                    { 5, "Herofall" },
                    { 6, "ScarsOfWar" },
                    { 7, "Frostheart" },
                    { 8, "DeadOfWinter" },
                    { 9, "Doombringer" }
                });

            migrationBuilder.InsertData(
                table: "StartCondition",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { 1, "WhenFull" },
                    { 2, "Scheduled" }
                });

            migrationBuilder.InsertData(
                table: "TournamentType",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Draft" },
                    { 2, "Sealed" },
                    { 3, "Standard" },
                    { 4, "Immortal" },
                    { 5, "Rock" },
                    { 6, "Iconoclast" }
                });
        }

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FriendStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FriendStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FriendStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MatchType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MatchType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MatchType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rank",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rank",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rank",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rank",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Rank",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Set",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "StartCondition",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StartCondition",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TournamentType",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TournamentType",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TournamentType",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TournamentType",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TournamentType",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "TournamentType",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
