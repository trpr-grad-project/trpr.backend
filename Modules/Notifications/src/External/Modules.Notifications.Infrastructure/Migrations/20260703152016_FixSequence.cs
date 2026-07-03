using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "latest_sequence_number",
                schema: "ntf",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_notifications_sequence_number",
                schema: "ntf",
                table: "notifications",
                column: "sequence_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_notifications_sequence_number",
                schema: "ntf",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "latest_sequence_number",
                schema: "ntf",
                table: "users");
        }
    }
}
