using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRowVersionToTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                schema: "ntf",
                table: "templates",
                type: "bytea",
                rowVersion: true,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "template_type",
                schema: "ntf",
                table: "templates",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "row_version",
                schema: "ntf",
                table: "templates");

            migrationBuilder.DropColumn(
                name: "template_type",
                schema: "ntf",
                table: "templates");
        }
    }
}
