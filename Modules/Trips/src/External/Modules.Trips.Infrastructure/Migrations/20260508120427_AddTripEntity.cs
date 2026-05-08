using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTripEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_name = table.Column<string>(type: "text", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Trips",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    theme_id = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    price = table.Column<double>(type: "double precision", precision: 10, scale: 2, nullable: false),
                    actual_duration = table.Column<double>(type: "double precision", nullable: false),
                    expected_duration = table.Column<double>(type: "double precision", nullable: false),
                    images = table.Column<string[]>(type: "text[]", nullable: false),
                    trip_visibility = table.Column<int>(type: "integer", nullable: false),
                    max_participants_count = table.Column<int>(type: "integer", nullable: false),
                    guide_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trips", x => x.id);
                    table.ForeignKey(
                        name: "fk_trips_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Days",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_days", x => x.id);
                    table.ForeignKey(
                        name: "fk_days_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "Trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_participant",
                schema: "trp",
                columns: table => new
                {
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_participant", x => new { x.trip_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_trip_participant_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "Trips",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_participant_user_user_id",
                        column: x => x.user_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place_day",
                schema: "trp",
                columns: table => new
                {
                    days_id = table.Column<Guid>(type: "uuid", nullable: false),
                    places_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_place_day", x => new { x.days_id, x.places_id });
                    table.ForeignKey(
                        name: "fk_place_day_day_days_id",
                        column: x => x.days_id,
                        principalSchema: "trp",
                        principalTable: "Days",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_place_day_places_places_id",
                        column: x => x.places_id,
                        principalSchema: "trp",
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_days_trip_id",
                schema: "trp",
                table: "Days",
                column: "trip_id");

            migrationBuilder.CreateIndex(
                name: "ix_place_day_places_id",
                schema: "trp",
                table: "place_day",
                column: "places_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_participant_user_id",
                schema: "trp",
                table: "trip_participant",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_trips_theme_id",
                schema: "trp",
                table: "Trips",
                column: "theme_id");

            migrationBuilder.CreateIndex(
                name: "ix_trips_user_id",
                schema: "trp",
                table: "Trips",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "place_day",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "trip_participant",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "Days",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "Trips",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "user",
                schema: "trp");
        }
    }
}
