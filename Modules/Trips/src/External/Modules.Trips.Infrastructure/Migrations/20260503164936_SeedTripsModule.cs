using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedTripsModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "trp",
                table: "tags",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                schema: "trp",
                table: "categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "food_drink" },
                    { 2, "entertainment" },
                    { 3, "culture" },
                    { 4, "activities" },
                    { 5, "outdoor" },
                    { 6, "relaxation" },
                    { 7, "other" }
                });

            migrationBuilder.InsertData(
                schema: "trp",
                table: "governorates",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "Aswan" },
                    { 2, "Asyut" },
                    { 3, "Luxor" },
                    { 4, "Alexandria" },
                    { 5, "Ismailia" },
                    { 6, "Suez" },
                    { 7, "Dakahlia" },
                    { 8, "Faiyum" },
                    { 9, "Cairo" },
                    { 10, "Giza" },
                    { 11, "Beheira" },
                    { 12, "Sharqia" },
                    { 13, "Gharbia" },
                    { 14, "Qalyubia" },
                    { 15, "Monufia" },
                    { 16, "Minya" },
                    { 17, "New Valley" },
                    { 18, "Beni Suef" },
                    { 19, "Port Said" },
                    { 20, "South Sinai" },
                    { 21, "Damietta" },
                    { 22, "Sohag" },
                    { 23, "North Sinai" },
                    { 24, "Qena" },
                    { 25, "Kafr El Sheikh" },
                    { 26, "Matruh" },
                    { 27, "Red Sea" }
                });

            migrationBuilder.InsertData(
                schema: "trp",
                table: "tags",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "cinema" },
                    { 2, "theatre" },
                    { 3, "live_shows" },
                    { 4, "nightlife" },
                    { 5, "concert_venue" },
                    { 6, "festival" },
                    { 7, "museum" },
                    { 8, "historical_site" },
                    { 9, "landmark" },
                    { 10, "religious_site" },
                    { 11, "art_gallery" },
                    { 12, "restaurant" },
                    { 13, "cafe" },
                    { 14, "street_food" },
                    { 15, "fine_dining" },
                    { 16, "fast_food" },
                    { 17, "rooftop" },
                    { 18, "family_friendly" },
                    { 19, "kid_friendly" },
                    { 20, "group_activities" },
                    { 21, "guided_tours" },
                    { 22, "adventure" },
                    { 23, "nature" },
                    { 24, "park" },
                    { 25, "beach" },
                    { 26, "desert" },
                    { 27, "river_view" },
                    { 28, "spa" },
                    { 29, "quiet_place" },
                    { 30, "luxury" },
                    { 31, "scenic" },
                    { 32, "romantic" },
                    { 33, "budget_friendly" },
                    { 34, "tourist_hotspot" },
                    { 35, "local_favorite" },
                    { 36, "instagrammable" },
                    { 37, "hidden_gem" },
                    { 38, "accessible" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_tags_name",
                schema: "trp",
                table: "tags",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_governorates_name",
                schema: "trp",
                table: "governorates",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_categories_name",
                schema: "trp",
                table: "categories",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_tags_name",
                schema: "trp",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "ix_governorates_name",
                schema: "trp",
                table: "governorates");

            migrationBuilder.DropIndex(
                name: "ix_categories_name",
                schema: "trp",
                table: "categories");

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "categories",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "categories",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "categories",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "categories",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "categories",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "categories",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "categories",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "governorates",
                keyColumn: "id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "tags",
                keyColumn: "id",
                keyValue: 38);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "trp",
                table: "tags",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
