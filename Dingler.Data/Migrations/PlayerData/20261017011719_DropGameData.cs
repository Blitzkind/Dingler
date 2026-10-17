using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Dingler.Data.Migrations.PlayerData
{
    /// <inheritdoc />
    public partial class DropGameData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "ATTACH DATABASE 'data/game.db' as game",
                suppressTransaction: true);
            
            migrationBuilder.Sql("INSERT OR IGNORE INTO game.TournamentType (Id, Name) SELECT Id, Name FROM TournamentType;");
            migrationBuilder.Sql("INSERT OR IGNORE INTO game.StartCondition (Id, Description) SELECT Id, Description FROM StartCondition;");
            migrationBuilder.Sql("INSERT OR IGNORE INTO game.MatchType (Id, Description) SELECT Id, Description FROM MatchType;");
            migrationBuilder.Sql(
                "INSERT OR IGNORE INTO game.Tournaments (Id, Description, NeededPlayers, StartDate, StartConditionId, TournamentTypeId, MatchTypeId, IsVisible, StateId) SELECT Id, Description, NeededPlayers, StartDate, StartConditionId, TournamentTypeId, MatchTypeId, 1, 1 FROM Tournaments;");
            migrationBuilder.Sql(
                "INSERT OR IGNORE INTO game.\"Set\" (\"Id\", \"Name\") SELECT \"Id\", \"Name\" FROM \"Set\";");
            migrationBuilder.Sql(
                "INSERT OR IGNORE INTO game.\"DraftSets\" (\"Id\", \"TournamentId\", \"CardSetId\", \"Order\") " +
                "SELECT \"Id\", \"TournamentId\", \"CardSetId\", \"Order\" FROM \"DraftSets\";");
            migrationBuilder.DropTable(
                name: "DraftSets");

            migrationBuilder.DropTable(
                name: "GameState");

            migrationBuilder.DropTable(
                name: "Set");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "MatchType");

            migrationBuilder.DropTable(
                name: "StartCondition");

            migrationBuilder.DropTable(
                name: "TournamentType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Dump = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MatchType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Set",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Set", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StartCondition",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartCondition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MatchTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartConditionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    NeededPlayers = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournaments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tournaments_MatchType_MatchTypeId",
                        column: x => x.MatchTypeId,
                        principalTable: "MatchType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tournaments_StartCondition_StartConditionId",
                        column: x => x.StartConditionId,
                        principalTable: "StartCondition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tournaments_TournamentType_TournamentTypeId",
                        column: x => x.TournamentTypeId,
                        principalTable: "TournamentType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DraftSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CardSetId = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DraftSets_Set_CardSetId",
                        column: x => x.CardSetId,
                        principalTable: "Set",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DraftSets_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.InsertData(
                table: "Tournaments",
                columns: new[] { "Id", "Description", "MatchTypeId", "NeededPlayers", "StartConditionId", "StartDate", "TournamentTypeId" },
                values: new object[,]
                {
                    { 1, "1v1 Standard Match", 1, 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 2, "1v1 Immortal Match", 1, 2, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 3, "8-Player Booster Draft", 2, 8, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.InsertData(
                table: "DraftSets",
                columns: new[] { "Id", "CardSetId", "Order", "TournamentId" },
                values: new object[,]
                {
                    { 1, 1, 1, 3 },
                    { 2, 2, 2, 3 },
                    { 3, 2, 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftSets_CardSetId",
                table: "DraftSets",
                column: "CardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftSets_TournamentId",
                table: "DraftSets",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchType_Description",
                table: "MatchType",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Set_Name",
                table: "Set",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StartCondition_Description",
                table: "StartCondition",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournament_Description",
                table: "Tournaments",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_MatchTypeId",
                table: "Tournaments",
                column: "MatchTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_StartConditionId",
                table: "Tournaments",
                column: "StartConditionId");

            migrationBuilder.CreateIndex(
                name: "IX_Tournaments_TournamentTypeId",
                table: "Tournaments",
                column: "TournamentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentType_Name",
                table: "TournamentType",
                column: "Name",
                unique: true);
        }
    }
}
