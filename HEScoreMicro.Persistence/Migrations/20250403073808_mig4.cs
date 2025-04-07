using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_About_Building_BuildingId",
                table: "About");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_Building_BuildingId",
                table: "Address");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Building",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Address",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "About",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_About_Building_BuildingId",
                table: "About",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Building_BuildingId",
                table: "Address",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_About_Building_BuildingId",
                table: "About");

            migrationBuilder.DropForeignKey(
                name: "FK_Address_Building_BuildingId",
                table: "Address");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Building",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "Address",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "BuildingId",
                table: "About",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_About_Building_BuildingId",
                table: "About",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Address_Building_BuildingId",
                table: "Address",
                column: "BuildingId",
                principalTable: "Building",
                principalColumn: "Id");
        }
    }
}
