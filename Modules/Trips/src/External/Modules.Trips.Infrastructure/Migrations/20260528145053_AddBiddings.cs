using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBiddings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Point>(
                name: "centroid",
                schema: "trp",
                table: "trip",
                type: "geography (Point, 4326)",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "trip_bidding",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    guide_id = table.Column<Guid>(type: "uuid", nullable: false),
                    proposed_price = table.Column<double>(type: "double precision", nullable: false),
                    proposal_message = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_bidding", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_bidding_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_bidding_user_guide_id",
                        column: x => x.guide_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_trip_centroid",
                schema: "trp",
                table: "trip",
                column: "centroid")
                .Annotation("Npgsql:IndexMethod", "GIST");

            migrationBuilder.CreateIndex(
                name: "ix_trip_bidding_guide_id",
                schema: "trp",
                table: "trip_bidding",
                column: "guide_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_bidding_trip_id_guide_id",
                schema: "trp",
                table: "trip_bidding",
                columns: new[] { "trip_id", "guide_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trip_bidding",
                schema: "trp");

            migrationBuilder.DropIndex(
                name: "ix_trip_centroid",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropColumn(
                name: "centroid",
                schema: "trp",
                table: "trip");
        }
    }
}
