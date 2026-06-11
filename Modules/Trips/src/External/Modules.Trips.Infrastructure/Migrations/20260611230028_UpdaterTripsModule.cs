using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdaterTripsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "rating",
                schema: "trp",
                table: "trip_participant",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "review",
                schema: "trp",
                table: "trip_participant",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                schema: "trp",
                table: "trip_participant");

            migrationBuilder.DropColumn(
                name: "review",
                schema: "trp",
                table: "trip_participant");
        }
    }
}
