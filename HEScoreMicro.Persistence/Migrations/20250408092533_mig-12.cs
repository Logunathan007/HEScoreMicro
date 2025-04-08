using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ZoneWindow_BuildingId",
                table: "ZoneWindow");

            migrationBuilder.DropIndex(
                name: "IX_ZoneWall_BuildingId",
                table: "ZoneWall");

            migrationBuilder.DropIndex(
                name: "IX_ZoneRoof_BuildingId",
                table: "ZoneRoof");

            migrationBuilder.DropIndex(
                name: "IX_ZoneFloor_BuildingId",
                table: "ZoneFloor");

            migrationBuilder.DropIndex(
                name: "IX_WaterHeater_BuildingId",
                table: "WaterHeater");

            migrationBuilder.DropIndex(
                name: "IX_PVSystem_BuildingId",
                table: "PVSystem");

            migrationBuilder.DropIndex(
                name: "IX_HeatingCoolingSystem_BuildingId",
                table: "HeatingCoolingSystem");

            migrationBuilder.DropIndex(
                name: "IX_About_BuildingId",
                table: "About");

            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "Address",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "YearBuilt",
                table: "About",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TotalConditionedFloorArea",
                table: "About",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StoriesAboveGroundLevel",
                table: "About",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfBedrooms",
                table: "About",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InteriorFloorToCeilingHeight",
                table: "About",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DirectionFacedByFrontOfHome",
                table: "About",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "About",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "BlowerDoorTestConducted",
                table: "About",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssessmentDate",
                table: "About",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ManufacturedHomeType",
                table: "About",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWindow_BuildingId",
                table: "ZoneWindow",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWall_BuildingId",
                table: "ZoneWall",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZoneRoof_BuildingId",
                table: "ZoneRoof",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ZoneFloor_BuildingId",
                table: "ZoneFloor",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WaterHeater_BuildingId",
                table: "WaterHeater",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PVSystem_BuildingId",
                table: "PVSystem",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HeatingCoolingSystem_BuildingId",
                table: "HeatingCoolingSystem",
                column: "BuildingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_About_BuildingId",
                table: "About",
                column: "BuildingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ZoneWindow_BuildingId",
                table: "ZoneWindow");

            migrationBuilder.DropIndex(
                name: "IX_ZoneWall_BuildingId",
                table: "ZoneWall");

            migrationBuilder.DropIndex(
                name: "IX_ZoneRoof_BuildingId",
                table: "ZoneRoof");

            migrationBuilder.DropIndex(
                name: "IX_ZoneFloor_BuildingId",
                table: "ZoneFloor");

            migrationBuilder.DropIndex(
                name: "IX_WaterHeater_BuildingId",
                table: "WaterHeater");

            migrationBuilder.DropIndex(
                name: "IX_PVSystem_BuildingId",
                table: "PVSystem");

            migrationBuilder.DropIndex(
                name: "IX_HeatingCoolingSystem_BuildingId",
                table: "HeatingCoolingSystem");

            migrationBuilder.DropIndex(
                name: "IX_About_BuildingId",
                table: "About");

            migrationBuilder.DropColumn(
                name: "ManufacturedHomeType",
                table: "About");

            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "Address",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "YearBuilt",
                table: "About",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "TotalConditionedFloorArea",
                table: "About",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "StoriesAboveGroundLevel",
                table: "About",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfBedrooms",
                table: "About",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "InteriorFloorToCeilingHeight",
                table: "About",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "DirectionFacedByFrontOfHome",
                table: "About",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "About",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<bool>(
                name: "BlowerDoorTestConducted",
                table: "About",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AssessmentDate",
                table: "About",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWindow_BuildingId",
                table: "ZoneWindow",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneWall_BuildingId",
                table: "ZoneWall",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneRoof_BuildingId",
                table: "ZoneRoof",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_ZoneFloor_BuildingId",
                table: "ZoneFloor",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_WaterHeater_BuildingId",
                table: "WaterHeater",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_PVSystem_BuildingId",
                table: "PVSystem",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_HeatingCoolingSystem_BuildingId",
                table: "HeatingCoolingSystem",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_About_BuildingId",
                table: "About",
                column: "BuildingId");
        }
    }
}
