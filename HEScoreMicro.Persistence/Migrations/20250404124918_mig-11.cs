using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeatingCoolingSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatingCoolingSystem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HeatingCoolingSystem_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Systems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HeatingCoolingSystemId = table.Column<Guid>(type: "uuid", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    PercentAreaServed = table.Column<double>(type: "double precision", nullable: false),
                    HeatingSystemType = table.Column<string>(type: "text", nullable: true),
                    KnowHeatingEfficiency = table.Column<bool>(type: "boolean", nullable: true),
                    HeatingSystemEfficiencyValue = table.Column<double>(type: "double precision", nullable: true),
                    HeatingSystemYearInstalled = table.Column<int>(type: "integer", nullable: true),
                    CoolingSystemType = table.Column<string>(type: "text", nullable: true),
                    KnowCoolingEfficiency = table.Column<bool>(type: "boolean", nullable: true),
                    CoolingSystemEfficiencyValue = table.Column<double>(type: "double precision", nullable: true),
                    CoolingSystemEfficiencyUnit = table.Column<string>(type: "text", nullable: true),
                    CoolingSystemYearInstalled = table.Column<int>(type: "integer", nullable: true),
                    DuctLeakageTestPerformed = table.Column<bool>(type: "boolean", nullable: true),
                    DuctAreProfessionallySealed = table.Column<bool>(type: "boolean", nullable: true),
                    DuctLocationCount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Systems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Systems_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Systems_HeatingCoolingSystem_HeatingCoolingSystemId",
                        column: x => x.HeatingCoolingSystemId,
                        principalTable: "HeatingCoolingSystem",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DuctLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemsId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    DuctsIsInsulated = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DuctLocation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DuctLocation_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DuctLocation_Systems_SystemsId",
                        column: x => x.SystemsId,
                        principalTable: "Systems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DuctLocation_BuildingId",
                table: "DuctLocation",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_DuctLocation_SystemsId",
                table: "DuctLocation",
                column: "SystemsId");

            migrationBuilder.CreateIndex(
                name: "IX_HeatingCoolingSystem_BuildingId",
                table: "HeatingCoolingSystem",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_BuildingId",
                table: "Systems",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_HeatingCoolingSystemId",
                table: "Systems",
                column: "HeatingCoolingSystemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DuctLocation");

            migrationBuilder.DropTable(
                name: "Systems");

            migrationBuilder.DropTable(
                name: "HeatingCoolingSystem");
        }
    }
}
