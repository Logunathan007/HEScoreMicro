using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZoneWindow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    WindowAreaFront = table.Column<double>(type: "double precision", nullable: true),
                    WindowAreaBack = table.Column<double>(type: "double precision", nullable: true),
                    WindowAreaLeft = table.Column<double>(type: "double precision", nullable: true),
                    WindowAreaRight = table.Column<double>(type: "double precision", nullable: true),
                    WindowsSame = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneWindow", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneWindow_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Window",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneWindowId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    SolarScreen = table.Column<bool>(type: "boolean", nullable: true),
                    KnowWindowSpecification = table.Column<bool>(type: "boolean", nullable: true),
                    UFactor = table.Column<double>(type: "double precision", nullable: true),
                    SHGC = table.Column<double>(type: "double precision", nullable: true),
                    Panes = table.Column<string>(type: "text", nullable: true),
                    FrameMaterial = table.Column<string>(type: "text", nullable: true),
                    GlazingType = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Window", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Window_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Window_ZoneWindow_ZoneWindowId",
                        column: x => x.ZoneWindowId,
                        principalTable: "ZoneWindow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Window_BuildingId",
                table: "Window",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Window_ZoneWindowId",
                table: "Window",
                column: "ZoneWindowId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWindow_BuildingId",
                table: "ZoneWindow",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Window");

            migrationBuilder.DropTable(
                name: "ZoneWindow");
        }
    }
}
