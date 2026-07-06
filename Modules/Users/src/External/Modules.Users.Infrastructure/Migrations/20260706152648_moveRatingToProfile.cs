using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class moveRatingToProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                schema: "usr",
                table: "users");

            migrationBuilder.DropColumn(
                name: "rating_count",
                schema: "usr",
                table: "users");

            migrationBuilder.AddColumn<double>(
                name: "rating",
                schema: "usr",
                table: "profiles",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "rating_count",
                schema: "usr",
                table: "profiles",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<List<string>>(
                name: "reviews",
                schema: "usr",
                table: "profiles",
                type: "text[]",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rating",
                schema: "usr",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "rating_count",
                schema: "usr",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "reviews",
                schema: "usr",
                table: "profiles");

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
    }
}
