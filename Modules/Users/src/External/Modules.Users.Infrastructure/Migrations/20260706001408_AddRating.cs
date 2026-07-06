using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "rating",
                schema: "usr",
                table: "users",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rating_count",
                schema: "usr",
                table: "users",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                schema: "usr",
                table: "users");

            migrationBuilder.DropColumn(
                name: "rating_count",
                schema: "usr",
                table: "users");
        }
    }
}
