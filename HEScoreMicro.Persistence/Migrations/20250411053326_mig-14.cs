using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig14 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearOfManufacture",
                table: "WaterHeater",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DuctLeakageTestValue",
                table: "Systems",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HeatingSystemEfficiencyUnit",
                table: "Systems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PercentageOfDucts",
                table: "DuctLocation",
                type: "double precision",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DuctLeakageTestValue",
                table: "Systems");

            migrationBuilder.DropColumn(
                name: "HeatingSystemEfficiencyUnit",
                table: "Systems");

            migrationBuilder.DropColumn(
                name: "PercentageOfDucts",
                table: "DuctLocation");

            migrationBuilder.AlterColumn<double>(
                name: "YearOfManufacture",
                table: "WaterHeater",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
