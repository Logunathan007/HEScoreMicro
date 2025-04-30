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
            migrationBuilder.DropForeignKey(
                name: "FK_About_Building_BuildingId",
                table: "About");

            migrationBuilder.DropForeignKey(
                name: "FK_DuctLocation_Building_BuildingId",
                table: "DuctLocation");

            migrationBuilder.DropForeignKey(
                name: "FK_RoofAttic_Building_BuildingId",
                table: "RoofAttic");

            migrationBuilder.DropForeignKey(
                name: "FK_Systems_Building_BuildingId",
                table: "Systems");

            migrationBuilder.DropForeignKey(
                name: "FK_Systems_HeatingCoolingSystem_HeatingCoolingSystemId",
                table: "Systems");

            migrationBuilder.DropForeignKey(
                name: "FK_Wall_Building_BuildingId",
                table: "Wall");

            migrationBuilder.DropForeignKey(
                name: "FK_Window_Building_BuildingId",
                table: "Window");

            migrationBuilder.DropIndex(
                name: "IX_Window_BuildingId",
                table: "Window");

            migrationBuilder.DropIndex(
                name: "IX_Wall_BuildingId",
                table: "Wall");

            migrationBuilder.DropIndex(
                name: "IX_Systems_BuildingId",
                table: "Systems");

            migrationBuilder.DropIndex(
                name: "IX_RoofAttic_BuildingId",
                table: "RoofAttic");

            migrationBuilder.DropIndex(
                name: "IX_DuctLocation_BuildingId",
                table: "DuctLocation");

            migrationBuilder.DropIndex(
                name: "IX_About_BuildingId",
                table: "About");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Window");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Wall");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Systems");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "RoofAttic");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "Foundation");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "DuctLocation");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "About");

            migrationBuilder.RenameColumn(
                name: "BuildingId",
                table: "Building",
                newName: "AboutId");

            migrationBuilder.AlterColumn<Guid>(
                name: "HeatingCoolingSystemId",
                table: "Systems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Building_AboutId",
                table: "Building",
                column: "AboutId");

            migrationBuilder.AddForeignKey(
                name: "FK_Building_About_AboutId",
                table: "Building",
                column: "AboutId",
                principalTable: "About",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Systems_HeatingCoolingSystem_HeatingCoolingSystemId",
                table: "Systems",
                column: "HeatingCoolingSystemId",
                principalTable: "HeatingCoolingSystem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Building_About_AboutId",
                table: "Building");

            migrationBuilder.DropForeignKey(
                name: "FK_Systems_HeatingCoolingSystem_HeatingCoolingSystemId",
                table: "Systems");

            migrationBuilder.DropIndex(
                name: "IX_Building_AboutId",
                table: "Building");

            migrationBuilder.RenameColumn(
                name: "AboutId",
                table: "Building",
                newName: "BuildingId");

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Window",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Wall",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "HeatingCoolingSystemId",
                table: "Systems",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Systems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "RoofAttic",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "Foundation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "DuctLocation",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "About",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Window_BuildingId",
                table: "Window",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Wall_BuildingId",
                table: "Wall",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_Systems_BuildingId",
                table: "Systems",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_RoofAttic_BuildingId",
                table: "RoofAttic",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_DuctLocation_BuildingId",
                table: "DuctLocation",
                column: "BuildingId");

            migrationBuilder.CreateIndex(
                name: "IX_About_BuildingId",
                table: "About",
                column: "BuildingId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_About_Building_BuildingId",
                table: "About",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DuctLocation_Building_BuildingId",
                table: "DuctLocation",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoofAttic_Building_BuildingId",
                table: "RoofAttic",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Systems_Building_BuildingId",
                table: "Systems",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Systems_HeatingCoolingSystem_HeatingCoolingSystemId",
                table: "Systems",
                column: "HeatingCoolingSystemId",
                principalTable: "HeatingCoolingSystem",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Wall_Building_BuildingId",
                table: "Wall",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Window_Building_BuildingId",
                table: "Window",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
