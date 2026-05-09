using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGovernoratesToTrips : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "order",
                schema: "trp",
                table: "day",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "trip_governorate",
                schema: "trp",
                columns: table => new
                {
                    trip_id = table.Column<Guid>(type: "uuid", nullable: false),
                    governorate_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_trip_governorate", x => new { x.trip_id, x.governorate_id });
                    table.ForeignKey(
                        name: "fk_trip_governorate_governorates_governorate_id",
                        column: x => x.governorate_id,
                        principalSchema: "trp",
                        principalTable: "governorates",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_trip_governorate_trip_trip_id",
                        column: x => x.trip_id,
                        principalSchema: "trp",
                        principalTable: "trip",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_trip_price",
                schema: "trp",
                table: "trip",
                column: "price");

            migrationBuilder.CreateIndex(
                name: "ix_trip_title",
                schema: "trp",
                table: "trip",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "ix_trip_governorate_governorate_id",
                schema: "trp",
                table: "trip_governorate",
                column: "governorate_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trip_governorate",
                schema: "trp");

            migrationBuilder.DropIndex(
                name: "ix_trip_price",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropIndex(
                name: "ix_trip_title",
                schema: "trp",
                table: "trip");

            migrationBuilder.DropColumn(
                name: "order",
                schema: "trp",
                table: "day");
        }
    }
}
