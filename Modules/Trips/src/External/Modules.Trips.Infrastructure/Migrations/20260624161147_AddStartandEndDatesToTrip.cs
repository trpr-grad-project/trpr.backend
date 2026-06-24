using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStartandEndDatesToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "end_date",
                schema: "trp",
                table: "trip",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "start_date",
                schema: "trp",
                table: "trip",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "day_date",
                schema: "trp",
                table: "day",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "end_date",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropColumn(
                name: "start_date",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropColumn(
                name: "day_date",
                schema: "trp",
                table: "day");
        }
    }
}
