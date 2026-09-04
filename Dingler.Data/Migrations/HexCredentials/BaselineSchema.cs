using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dingler.Data.Migrations.HexCredentials
{
        public partial class BaselineSchema : Migration
    {
                protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BannableOffenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Offense = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannableOffenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCredentials", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannedUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    UserCredentialsId = table.Column<int>(type: "INTEGER", nullable: false),
                    DateOfBan = table.Column<int>(type: "INTEGER", nullable: false),
                    LengthOfBan = table.Column<int>(type: "INTEGER", nullable: false),
                    OffenseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannedUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannedUsers_BannableOffenses_OffenseId",
                        column: x => x.OffenseId,
                        principalTable: "BannableOffenses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BannedUsers_UserCredentials_UserCredentialsId",
                        column: x => x.UserCredentialsId,
                        principalTable: "UserCredentials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLoginAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    FailedLoginCount = table.Column<int>(type: "INTEGER", nullable: true),
                    LastFailedLogin = table.Column<int>(type: "INTEGER", nullable: true),
                    LastSuccessfulLogin = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLoginAttempts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLoginAttempts_UserCredentials_UserId",
                        column: x => x.UserId,
                        principalTable: "UserCredentials",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannableOffenses_Offense",
                table: "BannableOffenses",
                column: "Offense",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannedUsers_OffenseId",
                table: "BannedUsers",
                column: "OffenseId");

            migrationBuilder.CreateIndex(
                name: "IX_BannedUsers_UserCredentialsId",
                table: "BannedUsers",
                column: "UserCredentialsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCredentials_Email",
                table: "UserCredentials",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginAttempts_UserId",
                table: "UserLoginAttempts",
                column: "UserId",
                unique: true);
        }

                protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannedUsers");

            migrationBuilder.DropTable(
                name: "UserLoginAttempts");

            migrationBuilder.DropTable(
                name: "BannableOffenses");

            migrationBuilder.DropTable(
                name: "UserCredentials");
        }
    }
}
