using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeGovernantPlaceSrid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Polygon>(
                name: "boundary",
                schema: "trp",
                table: "governorates",
                type: "geometry(Polygon,3857)",
                nullable: false,
                oldClrType: typeof(Polygon),
                oldType: "geometry(Polygon,4326)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Polygon>(
                name: "boundary",
                schema: "trp",
                table: "governorates",
                type: "geometry(Polygon,4326)",
                nullable: false,
                oldClrType: typeof(Polygon),
                oldType: "geometry(Polygon,3857)");
        }
    }
}
