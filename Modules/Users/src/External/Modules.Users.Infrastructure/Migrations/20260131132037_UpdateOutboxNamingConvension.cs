using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOutboxNamingConvension : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_consumer_message",
                schema: "usr",
                table: "outbox_consumer_message");

            migrationBuilder.RenameTable(
                name: "outbox_consumer_message",
                schema: "usr",
                newName: "outbox_consumer_messages",
                newSchema: "usr");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_consumer_messages",
                schema: "usr",
                table: "outbox_consumer_messages",
                columns: new[] { "id", "handler_name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_outbox_consumer_messages",
                schema: "usr",
                table: "outbox_consumer_messages");

            migrationBuilder.RenameTable(
                name: "outbox_consumer_messages",
                schema: "usr",
                newName: "outbox_consumer_message",
                newSchema: "usr");

            migrationBuilder.AddPrimaryKey(
                name: "pk_outbox_consumer_message",
                schema: "usr",
                table: "outbox_consumer_message",
                columns: new[] { "id", "handler_name" });
        }
    }
}
