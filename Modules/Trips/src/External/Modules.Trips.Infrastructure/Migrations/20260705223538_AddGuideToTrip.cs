using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGuideToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_trip_guide_id",
                schema: "trp",
                table: "trip",
                column: "guide_id");

            migrationBuilder.AddForeignKey(
                name: "fk_trip_user_guide_id",
                schema: "trp",
                table: "trip",
                column: "guide_id",
                principalSchema: "trp",
                principalTable: "user",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trip_user_guide_id",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropIndex(
                name: "ix_trip_guide_id",
                schema: "trp",
                table: "trip");
        }
    }
}
