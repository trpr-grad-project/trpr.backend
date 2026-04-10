using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseGeographyInsteadOfGeometry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                schema: "trp",
                table: "places",
                type: "geography (Point, 4326)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (Point, 4326)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                schema: "trp",
                table: "places",
                type: "geometry (Point, 4326)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geography (Point, 4326)");
        }
    }
}
