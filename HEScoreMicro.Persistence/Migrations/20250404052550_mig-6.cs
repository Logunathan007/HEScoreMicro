using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "RoofAttic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ZoneRoofId = table.Column<Guid>(type: "uuid", nullable: false),
                    AtticOrCeilingType = table.Column<string>(type: "text", nullable: true),
                    Construction = table.Column<string>(type: "text", nullable: true),
                    ExteriorFinish = table.Column<string>(type: "text", nullable: true),
                    CathedralCeilingArea = table.Column<double>(type: "double precision", nullable: true),
                    CathedralCeilingInsulation = table.Column<int>(type: "integer", nullable: true),
                    RoofInsulation = table.Column<int>(type: "integer", nullable: true),
                    RoofColor = table.Column<string>(type: "text", nullable: true),
                    Absorptance = table.Column<double>(type: "double precision", nullable: true),
                    SkylightsPresent = table.Column<bool>(type: "boolean", nullable: true),
                    SkylightArea = table.Column<double>(type: "double precision", nullable: true),
                    SolarScreen = table.Column<bool>(type: "boolean", nullable: true),
                    KnownSkylightSpecification = table.Column<bool>(type: "boolean", nullable: true),
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

            migrationBuilder.CreateIndex(
                name: "IX_RoofAttic_BuildingId",
                table: "RoofAttic",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_RoofAttic_ZoneRoofId",
                table: "RoofAttic",
                column: "ZoneRoofId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneRoof_BuildingId",
                table: "ZoneRoof",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoofAttic");

            migrationBuilder.DropTable(
                name: "ZoneRoof");
        }
    }
}
