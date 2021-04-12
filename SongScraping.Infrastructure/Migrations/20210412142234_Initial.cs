using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SongScraping.Infrastructure.Migrations
{
    public partial class Initial : Migration
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
                    ExternalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
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
                    Composer = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Album = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Genre = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Bpm = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    ExternalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Song", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ez2OnGameTrack",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Ez2OnDbSequenceNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    ExternalId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    SongId = table.Column<int>(type: "INTEGER", nullable: true),
                    DifficultyMode_Category = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: true),
                    DifficultyMode_Level = table.Column<int>(type: "INTEGER", maxLength: 5, nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    VisualizedBy = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ez2OnGameTrack", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ez2OnGameTrack_Song_SongId",
                        column: x => x.SongId,
                        principalTable: "Song",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "Index_Ez2OnGameTrack_WebApiLookupRef",
                table: "Ez2OnGameTrack",
                column: "ExternalId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ez2OnGameTrack_SongId",
                table: "Ez2OnGameTrack",
                column: "SongId");

            migrationBuilder.CreateIndex(
                name: "Index_Game_WebApiLookupRef",
                table: "Game",
                column: "ExternalId",
                unique: true);

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
                name: "Game");

            migrationBuilder.DropTable(
                name: "Song");
        }
    }
}
