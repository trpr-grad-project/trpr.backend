using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRolesToTrip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatorRole",
                schema: "trp",
                table: "trip",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "fk_trip_themes_theme_id",
                schema: "trp",
                table: "trip",
                column: "theme_id",
                principalSchema: "trp",
                principalTable: "themes",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_trip_themes_theme_id",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropColumn(
                name: "CreatorRole",
                schema: "trp",
                table: "trip");
        }
    }
}
