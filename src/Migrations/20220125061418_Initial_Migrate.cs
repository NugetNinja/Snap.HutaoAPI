using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Snap.Genshin.Website.Migrations
{
    public partial class Initial_Migrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    InnerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Uid = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.InnerId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    AppId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.AppId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AvatarDetails",
                columns: table => new
                {
                    InnerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AvatarId = table.Column<int>(type: "int", nullable: false),
                    AvatarLevel = table.Column<int>(type: "int", nullable: false),
                    ActivedConstellationNum = table.Column<int>(type: "int", nullable: false),
                    WeaponId = table.Column<int>(type: "int", nullable: false),
                    WeaponLevel = table.Column<int>(type: "int", nullable: false),
                    AffixLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarDetails", x => x.InnerId);
                    table.ForeignKey(
                        name: "FK_AvatarDetails_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "InnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PlayerRecords",
                columns: table => new
                {
                    InnerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UploadTime = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlayerId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerRecords", x => x.InnerId);
                    table.ForeignKey(
                        name: "FK_PlayerRecords_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "InnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClaimType = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClaimValue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "AppId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UsersSecrets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersSecrets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersSecrets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "AppId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ReliquarySetDetails",
                columns: table => new
                {
                    InnerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    AvatarDetailId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReliquarySetDetails", x => x.InnerId);
                    table.ForeignKey(
                        name: "FK_ReliquarySetDetails_AvatarDetails_AvatarDetailId",
                        column: x => x.AvatarDetailId,
                        principalTable: "AvatarDetails",
                        principalColumn: "InnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SpiralAbyssLevels",
                columns: table => new
                {
                    InnerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RecordId = table.Column<long>(type: "bigint", nullable: false),
                    FloorIndex = table.Column<int>(type: "int", nullable: false),
                    LevelIndex = table.Column<int>(type: "int", nullable: false),
                    Star = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpiralAbyssLevels", x => x.InnerId);
                    table.ForeignKey(
                        name: "FK_SpiralAbyssLevels_PlayerRecords_RecordId",
                        column: x => x.RecordId,
                        principalTable: "PlayerRecords",
                        principalColumn: "InnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SpiralAbyssBattles",
                columns: table => new
                {
                    InnerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BattleIndex = table.Column<int>(type: "int", nullable: false),
                    SpiralAbyssLevelId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpiralAbyssBattles", x => x.InnerId);
                    table.ForeignKey(
                        name: "FK_SpiralAbyssBattles_SpiralAbyssLevels_SpiralAbyssLevelId",
                        column: x => x.SpiralAbyssLevelId,
                        principalTable: "SpiralAbyssLevels",
                        principalColumn: "InnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SpiralAbyssAvatars",
                columns: table => new
                {
                    InnerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AvatarId = table.Column<int>(type: "int", nullable: false),
                    SpiralAbyssBattleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpiralAbyssAvatars", x => x.InnerId);
                    table.ForeignKey(
                        name: "FK_SpiralAbyssAvatars_SpiralAbyssBattles_SpiralAbyssBattleId",
                        column: x => x.SpiralAbyssBattleId,
                        principalTable: "SpiralAbyssBattles",
                        principalColumn: "InnerId",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AvatarDetails_PlayerId",
                table: "AvatarDetails",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerRecords_PlayerId",
                table: "PlayerRecords",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReliquarySetDetails_AvatarDetailId",
                table: "ReliquarySetDetails",
                column: "AvatarDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SpiralAbyssAvatars_SpiralAbyssBattleId",
                table: "SpiralAbyssAvatars",
                column: "SpiralAbyssBattleId");

            migrationBuilder.CreateIndex(
                name: "IX_SpiralAbyssBattles_SpiralAbyssLevelId",
                table: "SpiralAbyssBattles",
                column: "SpiralAbyssLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SpiralAbyssLevels_RecordId",
                table: "SpiralAbyssLevels",
                column: "RecordId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersClaims_UserId",
                table: "UsersClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSecrets_UserId",
                table: "UsersSecrets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReliquarySetDetails");

            migrationBuilder.DropTable(
                name: "SpiralAbyssAvatars");

            migrationBuilder.DropTable(
                name: "UsersClaims");

            migrationBuilder.DropTable(
                name: "UsersSecrets");

            migrationBuilder.DropTable(
                name: "AvatarDetails");

            migrationBuilder.DropTable(
                name: "SpiralAbyssBattles");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "SpiralAbyssLevels");

            migrationBuilder.DropTable(
                name: "PlayerRecords");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
