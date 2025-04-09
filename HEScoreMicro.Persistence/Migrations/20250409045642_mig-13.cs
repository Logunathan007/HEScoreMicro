using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HEScoreMicro.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class mig13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CathedralCeilingInsulation",
                table: "RoofAttic");

            migrationBuilder.RenameColumn(
                name: "CathedralCeilingArea",
                table: "RoofAttic",
                newName: "RoofArea");

            migrationBuilder.AddColumn<int>(
                name: "NumberofUnitsInBuilding",
                table: "About",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberofUnitsInBuilding",
                table: "About");

            migrationBuilder.RenameColumn(
                name: "RoofArea",
                table: "RoofAttic",
                newName: "CathedralCeilingArea");

            migrationBuilder.AddColumn<int>(
                name: "CathedralCeilingInsulation",
                table: "RoofAttic",
                type: "integer",
                nullable: true);
        }
    }
}
