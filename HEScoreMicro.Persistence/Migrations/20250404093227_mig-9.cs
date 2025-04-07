using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DYKnowWaterHeaterEnergyFactor",
                table: "WaterHeater",
                newName: "KnowWaterHeaterEnergyFactor");

            migrationBuilder.RenameColumn(
                name: "KnownSkylightSpecification",
                table: "RoofAttic",
                newName: "KnowSkylightSpecification");

            migrationBuilder.RenameColumn(
                name: "DYKnowSystemCapacity",
                table: "PVSystem",
                newName: "KnowSystemCapacity");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "KnowWaterHeaterEnergyFactor",
                table: "WaterHeater",
                newName: "DYKnowWaterHeaterEnergyFactor");

            migrationBuilder.RenameColumn(
                name: "KnowSkylightSpecification",
                table: "RoofAttic",
                newName: "KnownSkylightSpecification");

            migrationBuilder.RenameColumn(
                name: "KnowSystemCapacity",
                table: "PVSystem",
                newName: "DYKnowSystemCapacity");
        }
    }
}
