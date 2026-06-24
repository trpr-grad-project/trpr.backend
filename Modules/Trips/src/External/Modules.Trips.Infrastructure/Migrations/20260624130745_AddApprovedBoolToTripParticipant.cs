using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddApprovedBoolToTripParticipant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "rating",
                schema: "trp",
                table: "user",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rating_count",
                schema: "trp",
                table: "user",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "approved",
                schema: "trp",
                table: "trip_participant",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                schema: "trp",
                table: "user");

            migrationBuilder.DropColumn(
                name: "rating_count",
                schema: "trp",
                table: "user");

            migrationBuilder.DropColumn(
                name: "approved",
                schema: "trp",
                table: "trip_participant");
        }
    }
}
