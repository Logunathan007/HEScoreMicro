using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZoneFloor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnableSecondFoundation = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneFloor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneFloor_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Foundation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneFloorId = table.Column<Guid>(type: "uuid", nullable: false),
                    FoundationType = table.Column<string>(type: "text", nullable: true),
                    FoundationArea = table.Column<int>(type: "integer", nullable: true),
                    SlabInsulationLevel = table.Column<int>(type: "integer", nullable: true),
                    FloorInsulationLevel = table.Column<int>(type: "integer", nullable: true),
                    FoundationwallsInsulationLevel = table.Column<int>(type: "integer", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foundation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Foundation_ZoneFloor_ZoneFloorId",
                        column: x => x.ZoneFloorId,
                        principalTable: "ZoneFloor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Foundation_ZoneFloorId",
                table: "Foundation",
                column: "ZoneFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneFloor_BuildingId",
                table: "ZoneFloor",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Foundation");

            migrationBuilder.DropTable(
                name: "ZoneFloor");
        }
    }
}
