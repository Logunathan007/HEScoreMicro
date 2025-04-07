using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PVSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    HasPhotovoltaic = table.Column<bool>(type: "boolean", nullable: true),
                    YearInstalled = table.Column<int>(type: "integer", nullable: true),
                    DirectionPanelsFace = table.Column<string>(type: "text", nullable: true),
                    AnglePanelsAreTilted = table.Column<string>(type: "text", nullable: true),
                    DYKnowSystemCapacity = table.Column<bool>(type: "boolean", nullable: true),
                    NumberOfPanels = table.Column<int>(type: "integer", nullable: true),
                    DCCapacity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PVSystem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PVSystem_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WaterHeater",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    WaterHeaterType = table.Column<string>(type: "text", nullable: true),
                    DYKnowWaterHeaterEnergyFactor = table.Column<bool>(type: "boolean", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    EnergyValue = table.Column<double>(type: "double precision", nullable: true),
                    YearOfManufacture = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaterHeater", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WaterHeater_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PVSystem_BuildingId",
                table: "PVSystem",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterHeater_BuildingId",
                table: "WaterHeater",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PVSystem");

            migrationBuilder.DropTable(
                name: "WaterHeater");
        }
    }
}
