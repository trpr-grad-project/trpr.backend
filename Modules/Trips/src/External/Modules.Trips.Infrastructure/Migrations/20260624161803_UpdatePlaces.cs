using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePlaces : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                schema: "trp",
                table: "places",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_places_user_id",
                schema: "trp",
                table: "places",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_places_user_user_id",
                schema: "trp",
                table: "places",
                column: "user_id",
                principalSchema: "trp",
                principalTable: "user",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_places_user_user_id",
                schema: "trp",
                table: "places");

            migrationBuilder.DropIndex(
                name: "ix_places_user_id",
                schema: "trp",
                table: "places");

            migrationBuilder.DropColumn(
                name: "user_id",
                schema: "trp",
                table: "places");
        }
    }
}
