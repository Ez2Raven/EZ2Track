using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Migrations.GameMigrations
{
    public partial class EfNullableReferences : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    IsDlc = table.Column<bool>(type: "INTEGER", nullable: false),
                    UniversalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", rowVersion: true, nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Song",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Composer = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Album = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Bpm = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    UniversalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", rowVersion: true, nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Song", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GameTracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    DifficultyMode_Category = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    DifficultyMode_Level = table.Column<int>(type: "INTEGER", maxLength: 5, nullable: false),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    VisualizedBy = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    SongId = table.Column<int>(type: "INTEGER", nullable: false),
                    UniversalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameTracks_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameTracks_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Index_Game_WebApiLookupRef",
                table: "Game",
                column: "UniversalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GameTracks_GameId",
                table: "GameTracks",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_GameTracks_SongId",
                table: "GameTracks",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "Index_GameTracks_WebApiLookupRef",
                table: "GameTracks",
                column: "UniversalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_Song_WebApiLookupRef",
                table: "Song",
                column: "UniversalId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameTracks");

            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "Song");
        }
    }
}
