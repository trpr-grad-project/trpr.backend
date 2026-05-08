using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Trips.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedThemeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "trp",
                table: "themes",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "History" },
                    { 2, "Adventure" },
                    { 3, "Nature" },
                    { 4, "Culture" },
                    { 5, "Foodie" },
                    { 6, "Wellness" },
                    { 7, "Romantic" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "trp",
                table: "themes",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "themes",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "themes",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "themes",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "themes",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "themes",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "trp",
                table: "themes",
                keyColumn: "id",
                keyValue: 7);
        }
    }
}
