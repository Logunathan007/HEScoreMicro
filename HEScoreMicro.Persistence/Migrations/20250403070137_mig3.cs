using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Building",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "About",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AssessmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Comments = table.Column<string>(type: "text", nullable: true),
                    YearBuilt = table.Column<int>(type: "integer", nullable: true),
                    NumberOfBedrooms = table.Column<int>(type: "integer", nullable: true),
                    StoriesAboveGroundLevel = table.Column<int>(type: "integer", nullable: true),
                    InteriorFloorToCeilingHeight = table.Column<int>(type: "integer", nullable: true),
                    TotalConditionedFloorArea = table.Column<int>(type: "integer", nullable: true),
                    DirectionFacedByFrontOfHome = table.Column<string>(type: "text", nullable: true),
                    BlowerDoorTestConducted = table.Column<bool>(type: "boolean", nullable: true),
                    AirLeakageRate = table.Column<int>(type: "integer", nullable: true),
                    AirSealed = table.Column<bool>(type: "boolean", nullable: true),
                    BuildingId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_About", x => x.Id);
                    table.ForeignKey(
                        name: "FK_About_Building_BuildingId",
                        column: x => x.BuildingId,
                        principalTable: "Building",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_About_BuildingId",
                table: "About",
                column: "BuildingId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "About");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Building");
        }
    }
}
