using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNotificationsEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notify_email",
                schema: "ntf",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "notify_phone",
                schema: "ntf",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "notify_system",
                schema: "ntf",
                table: "notifications");

            migrationBuilder.AddColumn<int>(
                name: "sequence_number",
                schema: "ntf",
                table: "notifications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "title",
                schema: "ntf",
                table: "notifications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sequence_number",
                schema: "ntf",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "title",
                schema: "ntf",
                table: "notifications");

            migrationBuilder.AddColumn<bool>(
                name: "notify_email",
                schema: "ntf",
                table: "notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "notify_phone",
                schema: "ntf",
                table: "notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "notify_system",
                schema: "ntf",
                table: "notifications",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
