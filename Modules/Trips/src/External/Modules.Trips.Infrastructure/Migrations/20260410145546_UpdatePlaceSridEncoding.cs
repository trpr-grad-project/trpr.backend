using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlaceSridEncoding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_governorates_boundary",
                schema: "trp",
                table: "governorates");

            migrationBuilder.DropColumn(
                name: "boundary",
                schema: "trp",
                table: "governorates");

            migrationBuilder.AlterColumn<Point>(
                name: "location",
                schema: "trp",
                table: "places",
                type: "geometry (Point, 4326)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (Point, 3857)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Point>(
                name: "location",
                schema: "trp",
                table: "places",
                type: "geometry (Point, 3857)",
                nullable: false,
                oldClrType: typeof(Point),
                oldType: "geometry (Point, 4326)");

            migrationBuilder.AddColumn<MultiPolygon>(
                name: "boundary",
                schema: "trp",
                table: "governorates",
                type: "geometry(MultiPolygon,3857)",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "ix_governorates_boundary",
                schema: "trp",
                table: "governorates",
                column: "boundary")
                .Annotation("Npgsql:IndexMethod", "GIST");
        }
    }
}
