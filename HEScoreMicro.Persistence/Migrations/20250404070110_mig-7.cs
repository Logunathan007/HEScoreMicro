using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZoneWall",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExteriorWallSame = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneWall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneWall_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wall",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneWallId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Construction = table.Column<string>(type: "text", nullable: true),
                    ExteriorFinish = table.Column<string>(type: "text", nullable: true),
                    WallInsulationLevel = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wall", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wall_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wall_ZoneWall_ZoneWallId",
                        column: x => x.ZoneWallId,
                        principalTable: "ZoneWall",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wall_BuildingId",
                table: "Wall",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Wall_ZoneWallId",
                table: "Wall",
                column: "ZoneWallId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWall_BuildingId",
                table: "ZoneWall",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wall");

            migrationBuilder.DropTable(
                name: "ZoneWall");
        }
    }
}
