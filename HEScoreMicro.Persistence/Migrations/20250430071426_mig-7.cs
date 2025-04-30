using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Building_About_AboutId",
                table: "Building");

            migrationBuilder.DropIndex(
                name: "IX_Building_AboutId",
                table: "Building");

            migrationBuilder.DropColumn(
                name: "AboutId",
                table: "Building");

            migrationBuilder.AddColumn<Guid>(
                name: "BuildingId",
                table: "About",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_About_Building_BuildingId",
                table: "About");

            migrationBuilder.DropIndex(
                name: "IX_About_BuildingId",
                table: "About");

            migrationBuilder.DropColumn(
                name: "BuildingId",
                table: "About");

            migrationBuilder.AddColumn<Guid>(
                name: "AboutId",
                table: "Building",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

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
        }
    }
}
