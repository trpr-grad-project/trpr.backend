using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPlacesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"UPDATE trp.governorates 
                            SET name = CASE id
                                WHEN 1 THEN 'aswan'
                                WHEN 2 THEN 'asyut'
                                WHEN 3 THEN 'luxor'
                                WHEN 4 THEN 'alexandria'
                                WHEN 5 THEN 'ismailia'
                                WHEN 6 THEN 'suez'
                                WHEN 7 THEN 'dakahlia'
                                WHEN 8 THEN 'faiyum'
                                WHEN 9 THEN 'cairo'
                                WHEN 10 THEN 'giza'
                                WHEN 11 THEN 'beheira'
                                WHEN 12 THEN 'sharqia'
                                WHEN 13 THEN 'gharbia'
                                WHEN 14 THEN 'qalyubia'
                                WHEN 15 THEN 'monufia'
                                WHEN 16 THEN 'minya'
                                WHEN 17 THEN 'new valley'
                                WHEN 18 THEN 'beni suef'
                                WHEN 19 THEN 'port said'
                                WHEN 20 THEN 'south sinai'
                                WHEN 21 THEN 'damietta'
                                WHEN 22 THEN 'sohag'
                                WHEN 23 THEN 'north sinai'
                                WHEN 24 THEN 'qena'
                                WHEN 25 THEN 'kafr el sheikh'
                                WHEN 26 THEN 'matruh'
                                WHEN 27 THEN 'red sea'
                            END
                            WHERE id BETWEEN 1 AND 27;

                            INSERT INTO trp.governorates (name)
                            VALUES ('')
                            ON CONFLICT (name) DO NOTHING;");


            migrationBuilder.RenameTable(
                name: "Trips",
                schema: "trp",
                newName: "trip",
                newSchema: "trp");

            migrationBuilder.RenameTable(
                name: "Days",
                schema: "trp",
                newName: "day",
                newSchema: "trp");

            migrationBuilder.RenameIndex(
                name: "ix_trips_user_id",
                schema: "trp",
                table: "trip",
                newName: "ix_trip_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_trips_theme_id",
                schema: "trp",
                table: "trip",
                newName: "ix_trip_theme_id");

            migrationBuilder.RenameIndex(
                name: "ix_days_trip_id",
                schema: "trp",
                table: "day",
                newName: "ix_day_trip_id");

            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.RenameTable(
                name: "trip",
                schema: "trp",
                newName: "Trips",
                newSchema: "trp");

            migrationBuilder.RenameTable(
                name: "day",
                schema: "trp",
                newName: "Days",
                newSchema: "trp");

            migrationBuilder.RenameIndex(
                name: "ix_trip_user_id",
                schema: "trp",
                table: "Trips",
                newName: "ix_trips_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_trip_theme_id",
                schema: "trp",
                table: "Trips",
                newName: "ix_trips_theme_id");

            migrationBuilder.RenameIndex(
                name: "ix_day_trip_id",
                schema: "trp",
                table: "Days",
                newName: "ix_days_trip_id");
        }
    }
}
