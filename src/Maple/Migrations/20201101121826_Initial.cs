using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Maple.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AudioDeviceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    UpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    DeviceType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioDeviceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Playlists",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    UpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    Thumbnail = table.Column<string>(maxLength: 260, nullable: false),
                    IsShuffeling = table.Column<bool>(nullable: false),
                    PrivacyStatus = table.Column<int>(nullable: false),
                    RepeatMode = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Playlists", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AudioDevices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    UpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    OsId = table.Column<string>(nullable: false),
                    AudioDeviceTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AudioDevices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AudioDevices_AudioDeviceTypes_AudioDeviceTypeId",
                        column: x => x.AudioDeviceTypeId,
                        principalTable: "AudioDeviceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MediaItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    UpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    Location = table.Column<string>(maxLength: 2048, nullable: false),
                    Thumbnail = table.Column<string>(nullable: true),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    PrivacyStatus = table.Column<int>(nullable: false),
                    MediaItemType = table.Column<int>(nullable: false),
                    PlaylistId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaItems_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Mediaplayers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Sequence = table.Column<int>(nullable: false, defaultValue: 0)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CreatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    UpdatedOn = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedBy = table.Column<string>(nullable: true, defaultValue: "SYSTEM"),
                    IsPrimary = table.Column<bool>(nullable: false),
                    PlaylistId = table.Column<int>(nullable: true),
                    AudioDeviceId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mediaplayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mediaplayers_AudioDevices_AudioDeviceId",
                        column: x => x.AudioDeviceId,
                        principalTable: "AudioDevices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mediaplayers_Playlists_PlaylistId",
                        column: x => x.PlaylistId,
                        principalTable: "Playlists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AudioDeviceTypes",
                columns: new[] { "Id", "DeviceType", "Name", "Sequence" },
                values: new object[] { 1, 1, "WaveOut", 1 });

            migrationBuilder.InsertData(
                table: "AudioDeviceTypes",
                columns: new[] { "Id", "DeviceType", "Name", "Sequence" },
                values: new object[] { 2, 2, "DirectSound", 2 });

            migrationBuilder.InsertData(
                table: "AudioDeviceTypes",
                columns: new[] { "Id", "DeviceType", "Name", "Sequence" },
                values: new object[] { 3, 3, "WASAPI", 3 });

            migrationBuilder.InsertData(
                table: "AudioDeviceTypes",
                columns: new[] { "Id", "DeviceType", "Name", "Sequence" },
                values: new object[] { 4, 4, "ASIO", 4 });

            migrationBuilder.CreateIndex(
                name: "IX_AudioDevices_AudioDeviceTypeId",
                table: "AudioDevices",
                column: "AudioDeviceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaItems_PlaylistId",
                table: "MediaItems",
                column: "PlaylistId");

            migrationBuilder.CreateIndex(
                name: "IX_Mediaplayers_AudioDeviceId",
                table: "Mediaplayers",
                column: "AudioDeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Mediaplayers_PlaylistId",
                table: "Mediaplayers",
                column: "PlaylistId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MediaItems");

            migrationBuilder.DropTable(
                name: "Mediaplayers");

            migrationBuilder.DropTable(
                name: "AudioDevices");

            migrationBuilder.DropTable(
                name: "Playlists");

            migrationBuilder.DropTable(
                name: "AudioDeviceTypes");
        }
    }
}
