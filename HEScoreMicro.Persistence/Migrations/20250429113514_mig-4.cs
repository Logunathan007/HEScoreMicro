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
            migrationBuilder.DropIndex(
                name: "IX_EnergyStar_BuildingId",
                table: "EnergyStar");

            migrationBuilder.AddColumn<bool>(
                name: "EnergyStarPresent",
                table: "EnergyStar",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EnergyStar_BuildingId",
                table: "EnergyStar",
                column: "BuildingId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EnergyStar_BuildingId",
                table: "EnergyStar");

            migrationBuilder.DropColumn(
                name: "EnergyStarPresent",
                table: "EnergyStar");

            migrationBuilder.CreateIndex(
                name: "IX_EnergyStar_BuildingId",
                table: "EnergyStar",
                column: "BuildingId");
        }
    }
}
