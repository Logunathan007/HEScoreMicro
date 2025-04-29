using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Building",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Building", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "About",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    YearBuilt = table.Column<int>(type: "integer", nullable: false),
                    NumberOfBedrooms = table.Column<int>(type: "integer", nullable: false),
                    StoriesAboveGroundLevel = table.Column<int>(type: "integer", nullable: false),
                    InteriorFloorToCeilingHeight = table.Column<int>(type: "integer", nullable: false),
                    TotalConditionedFloorArea = table.Column<int>(type: "integer", nullable: false),
                    DirectionFacedByFrontOfHome = table.Column<string>(type: "text", nullable: false),
                    BlowerDoorTestConducted = table.Column<bool>(type: "boolean", nullable: false),
                    NumberofUnitsInBuilding = table.Column<int>(type: "integer", nullable: true),
                    ManufacturedHomeType = table.Column<string>(type: "text", nullable: true),
                    AirLeakageRate = table.Column<int>(type: "integer", nullable: true),
                    AirSealed = table.Column<bool>(type: "boolean", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_About", x => x.Id);
                    table.ForeignKey(
                        name: "FK_About_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DwellingUnitType = table.Column<string>(type: "text", nullable: true),
                    StreetAddress = table.Column<string>(type: "text", nullable: true),
                    AddressLine = table.Column<string>(type: "text", nullable: true),
                    City = table.Column<string>(type: "text", nullable: true),
                    State = table.Column<string>(type: "text", nullable: true),
                    ZipCode = table.Column<int>(type: "integer", nullable: false),
                    AssessmentType = table.Column<string>(type: "text", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Address_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "PVSystem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    HasPhotovoltaic = table.Column<bool>(type: "boolean", nullable: true),
                    YearInstalled = table.Column<int>(type: "integer", nullable: true),
                    DirectionPanelsFace = table.Column<string>(type: "text", nullable: true),
                    AnglePanelsAreTilted = table.Column<string>(type: "text", nullable: true),
                    KnowSystemCapacity = table.Column<bool>(type: "boolean", nullable: true),
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
                    KnowWaterHeaterEnergyFactor = table.Column<bool>(type: "boolean", nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    EnergyValue = table.Column<double>(type: "double precision", nullable: true),
                    YearOfManufacture = table.Column<int>(type: "integer", nullable: true)
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
                name: "ZoneRoof",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    EnableSecondRoofAttic = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneRoof", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZoneRoof_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "Systems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HeatingCoolingSystemId = table.Column<Guid>(type: "uuid", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    PercentAreaServed = table.Column<double>(type: "double precision", nullable: true),
                    HeatingSystemType = table.Column<string>(type: "text", nullable: true),
                    KnowHeatingEfficiency = table.Column<bool>(type: "boolean", nullable: true),
                    HeatingSystemEfficiencyUnit = table.Column<string>(type: "text", nullable: true),
                    HeatingSystemEfficiencyValue = table.Column<double>(type: "double precision", nullable: true),
                    HeatingSystemYearInstalled = table.Column<int>(type: "integer", nullable: true),
                    CoolingSystemType = table.Column<string>(type: "text", nullable: true),
                    KnowCoolingEfficiency = table.Column<bool>(type: "boolean", nullable: true),
                    CoolingSystemEfficiencyValue = table.Column<double>(type: "double precision", nullable: true),
                    CoolingSystemEfficiencyUnit = table.Column<string>(type: "text", nullable: true),
                    CoolingSystemYearInstalled = table.Column<int>(type: "integer", nullable: true),
                    DuctLeakageTestPerformed = table.Column<bool>(type: "boolean", nullable: true),
                    DuctLeakageTestValue = table.Column<double>(type: "double precision", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "RoofAttic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneRoofId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtticOrCeilingType = table.Column<string>(type: "text", nullable: true),
                    Construction = table.Column<string>(type: "text", nullable: true),
                    ExteriorFinish = table.Column<string>(type: "text", nullable: true),
                    RoofArea = table.Column<double>(type: "double precision", nullable: true),
                    RoofInsulation = table.Column<int>(type: "integer", nullable: true),
                    RoofColor = table.Column<string>(type: "text", nullable: true),
                    Absorptance = table.Column<double>(type: "double precision", nullable: true),
                    SkylightsPresent = table.Column<bool>(type: "boolean", nullable: true),
                    SkylightArea = table.Column<double>(type: "double precision", nullable: true),
                    SolarScreen = table.Column<bool>(type: "boolean", nullable: true),
                    KnowSkylightSpecification = table.Column<bool>(type: "boolean", nullable: true),
                    UFactor = table.Column<double>(type: "double precision", nullable: true),
                    SHGC = table.Column<double>(type: "double precision", nullable: true),
                    Panes = table.Column<string>(type: "text", nullable: true),
                    FrameMaterial = table.Column<string>(type: "text", nullable: true),
                    GlazingType = table.Column<string>(type: "text", nullable: true),
                    AtticFloorArea = table.Column<double>(type: "double precision", nullable: true),
                    AtticFloorInsulation = table.Column<int>(type: "integer", nullable: true),
                    KneeWallPresent = table.Column<bool>(type: "boolean", nullable: true),
                    KneeWallArea = table.Column<double>(type: "double precision", nullable: true),
                    KneeWallInsulation = table.Column<int>(type: "integer", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoofAttic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoofAttic_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoofAttic_ZoneRoof_ZoneRoofId",
                        column: x => x.ZoneRoofId,
                        principalTable: "ZoneRoof",
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
                    AdjacentTo = table.Column<string>(type: "text", nullable: true),
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

            migrationBuilder.CreateTable(
                name: "DuctLocation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemsId = table.Column<Guid>(type: "uuid", nullable: false),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: false),
                    Location = table.Column<string>(type: "text", nullable: true),
                    PercentageOfDucts = table.Column<double>(type: "double precision", nullable: true),
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
                name: "IX_About_BuildingId",
                table: "About",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Address_BuildingId",
                table: "Address",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DuctLocation_BuildingId",
                table: "DuctLocation",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_DuctLocation_SystemsId",
                table: "DuctLocation",
                column: "SystemsId");

            migrationBuilder.CreateIndex(
                name: "IX_Foundation_ZoneFloorId",
                table: "Foundation",
                column: "ZoneFloorId");

            migrationBuilder.CreateIndex(
                name: "IX_HeatingCoolingSystem_BuildingId",
                table: "HeatingCoolingSystem",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PVSystem_BuildingId",
                table: "PVSystem",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoofAttic_BuildingId",
                table: "RoofAttic",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_RoofAttic_ZoneRoofId",
                table: "RoofAttic",
                column: "ZoneRoofId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_BuildingId",
                table: "Systems",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_HeatingCoolingSystemId",
                table: "Systems",
                column: "HeatingCoolingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Wall_BuildingId",
                table: "Wall",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Wall_ZoneWallId",
                table: "Wall",
                column: "ZoneWallId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterHeater_BuildingId",
                table: "WaterHeater",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Window_BuildingId",
                table: "Window",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Window_ZoneWindowId",
                table: "Window",
                column: "ZoneWindowId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneFloor_BuildingId",
                table: "ZoneFloor",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZoneRoof_BuildingId",
                table: "ZoneRoof",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWall_BuildingId",
                table: "ZoneWall",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWindow_BuildingId",
                table: "ZoneWindow",
                column: "BuildingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "About");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "DuctLocation");

            migrationBuilder.DropTable(
                name: "Foundation");

            migrationBuilder.DropTable(
                name: "PVSystem");

            migrationBuilder.DropTable(
                name: "RoofAttic");

            migrationBuilder.DropTable(
                name: "Wall");

            migrationBuilder.DropTable(
                name: "WaterHeater");

            migrationBuilder.DropTable(
                name: "Window");

            migrationBuilder.DropTable(
                name: "Systems");

            migrationBuilder.DropTable(
                name: "ZoneFloor");

            migrationBuilder.DropTable(
                name: "ZoneRoof");

            migrationBuilder.DropTable(
                name: "ZoneWall");

            migrationBuilder.DropTable(
                name: "ZoneWindow");

            migrationBuilder.DropTable(
                name: "HeatingCoolingSystem");

            migrationBuilder.DropTable(
                name: "Building");
        }
    }
}
