using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ConvertPolygonToMultiPolygon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<MultiPolygon>(
                name: "boundary",
                schema: "trp",
                table: "governorates",
                type: "geometry(MultiPolygon,3857)",
                nullable: false,
                oldClrType: typeof(Polygon),
                oldType: "geometry(Polygon,3857)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Polygon>(
                name: "boundary",
                schema: "trp",
                table: "governorates",
                type: "geometry(Polygon,3857)",
                nullable: false,
                oldClrType: typeof(MultiPolygon),
                oldType: "geometry(MultiPolygon,3857)");
        }
    }
}
