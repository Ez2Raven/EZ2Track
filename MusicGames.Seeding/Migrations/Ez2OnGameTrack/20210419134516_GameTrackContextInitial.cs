using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MusicGames.Seeding.Migrations.Ez2OnGameTrack
{
    public partial class GameTrackContextInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Song",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                    Composer = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Album = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Bpm = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    ExternalId = table.Column<Guid>(type: "TEXT", nullable: false),
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
                    DifficultyMode_Category = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: true),
                    DifficultyMode_Level = table.Column<int>(type: "INTEGER", maxLength: 5, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    VisualizedBy = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    SongId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameTracks_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ez2OnGameTrack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ez2OnDbSequenceNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ez2OnGameTrack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ez2OnGameTrack_GameTracks_Id",
                        column: x => x.Id,
                        principalTable: "GameTracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameTracks_SongId",
                table: "GameTracks",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "Index_Song_WebApiLookupRef",
                table: "Song",
                column: "ExternalId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ez2OnGameTrack");

            migrationBuilder.DropTable(
                name: "GameTracks");

            migrationBuilder.DropTable(
                name: "Song");
        }
    }
}
