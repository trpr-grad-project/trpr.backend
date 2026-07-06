using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "average_rating",
                schema: "trp",
                table: "trip",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "total_ratings",
                schema: "trp",
                table: "trip",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "trip_rating",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    review = table.Column<string>(type: "text", nullable: true),
                    trip_id1 = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_rating", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_rating_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_rating_trip_trip_id1",
                        column: x => x.trip_id1,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_trip_rating_user_reviewer_id",
                        column: x => x.reviewer_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "trip_review",
                schema: "trp",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    reviewee_id = table.Column<Guid>(type: "uuid", nullable: false),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    review = table.Column<string>(type: "text", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_review", x => x.id);
                    table.ForeignKey(
                        name: "fk_trip_review_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_review_user_reviewee_id",
                        column: x => x.reviewee_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_review_user_reviewer_id",
                        column: x => x.reviewer_id,
                        principalSchema: "trp",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_trip_rating_reviewer_id",
                schema: "trp",
                table: "trip_rating",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_rating_trip_id_reviewer_id",
                schema: "trp",
                table: "trip_rating",
                columns: new[] { "trip_id", "reviewer_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_trip_rating_trip_id1",
                schema: "trp",
                table: "trip_rating",
                column: "trip_id1");

            migrationBuilder.CreateIndex(
                name: "ix_trip_review_reviewee_id",
                schema: "trp",
                table: "trip_review",
                column: "reviewee_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_review_reviewer_id",
                schema: "trp",
                table: "trip_review",
                column: "reviewer_id");

            migrationBuilder.CreateIndex(
                name: "ix_trip_review_trip_id_reviewer_id_reviewee_id",
                schema: "trp",
                table: "trip_review",
                columns: new[] { "trip_id", "reviewer_id", "reviewee_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trip_rating",
                schema: "trp");

            migrationBuilder.DropTable(
                name: "trip_review",
                schema: "trp");

            migrationBuilder.DropColumn(
                name: "average_rating",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropColumn(
                name: "total_ratings",
                schema: "trp",
                table: "trip");
        }
    }
}
