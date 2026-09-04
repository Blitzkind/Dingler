using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dingler.Data.Migrations.GameData
{
        public partial class BaselineSchema : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FriendStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendStatus", x => x.Id);
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
                name: "Rank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rank", x => x.Id);
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
                name: "PlayerProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    ELO = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1000),
                    RankId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerProfiles_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerProfiles_Rank_RankId",
                        column: x => x.RankId,
                        principalTable: "Rank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tournaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    NeededPlayers = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartConditionId = table.Column<int>(type: "INTEGER", nullable: false),
                    TournamentTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    MatchTypeId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "Decks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DeckName = table.Column<string>(type: "TEXT", nullable: false),
                    DeckGuid = table.Column<string>(type: "TEXT", nullable: false),
                    PlayerProfileId = table.Column<int>(type: "INTEGER", nullable: false),
                    ChampionGuid = table.Column<string>(type: "TEXT", nullable: false, defaultValue: "00000000-0000-0000-0000-000000000000"),
                    DeckBitsJson = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Decks_PlayerProfiles_PlayerProfileId",
                        column: x => x.PlayerProfileId,
                        principalTable: "PlayerProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequesterId = table.Column<int>(type: "INTEGER", nullable: false),
                    RequestedId = table.Column<int>(type: "INTEGER", nullable: false),
                    FriendStatusId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friends_FriendStatus_FriendStatusId",
                        column: x => x.FriendStatusId,
                        principalTable: "FriendStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friends_PlayerProfiles_RequestedId",
                        column: x => x.RequestedId,
                        principalTable: "PlayerProfiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friends_PlayerProfiles_RequesterId",
                        column: x => x.RequesterId,
                        principalTable: "PlayerProfiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DraftSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TournamentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CardSetId = table.Column<int>(type: "INTEGER", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Email",
                table: "Accounts",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_DeckGuid",
                table: "Decks",
                column: "DeckGuid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_DeckName_PlayerProfileId",
                table: "Decks",
                columns: new[] { "DeckName", "PlayerProfileId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Decks_PlayerProfileId",
                table: "Decks",
                column: "PlayerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftSets_CardSetId",
                table: "DraftSets",
                column: "CardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DraftSets_TournamentId",
                table: "DraftSets",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_FriendStatusId",
                table: "Friends",
                column: "FriendStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_RequestedId",
                table: "Friends",
                column: "RequestedId");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_RequesterId_RequestedId",
                table: "Friends",
                columns: new[] { "RequesterId", "RequestedId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendStatus_Description",
                table: "FriendStatus",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MatchType_Description",
                table: "MatchType",
                column: "Description",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProfiles_AccountId",
                table: "PlayerProfiles",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProfiles_RankId",
                table: "PlayerProfiles",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerProfiles_Username",
                table: "PlayerProfiles",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rank_Name",
                table: "Rank",
                column: "Name",
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

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "DraftSets");

            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DropTable(
                name: "Set");

            migrationBuilder.DropTable(
                name: "Tournaments");

            migrationBuilder.DropTable(
                name: "FriendStatus");

            migrationBuilder.DropTable(
                name: "PlayerProfiles");

            migrationBuilder.DropTable(
                name: "MatchType");

            migrationBuilder.DropTable(
                name: "StartCondition");

            migrationBuilder.DropTable(
                name: "TournamentType");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Rank");
        }
    }
}
