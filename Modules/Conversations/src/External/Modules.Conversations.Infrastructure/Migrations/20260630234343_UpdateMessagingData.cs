using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Conversations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessagingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "message_attachments",
                schema: "cnv");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "cnv",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "type",
                schema: "cnv",
                table: "conversations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "type",
                schema: "cnv",
                table: "messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "type",
                schema: "cnv",
                table: "conversations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "message_attachments",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    attachment_name = table.Column<string>(type: "text", nullable: false),
                    attachment_size = table.Column<long>(type: "bigint", nullable: false),
                    attachment_type = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_message_attachments", x => x.id);
                    table.ForeignKey(
                        name: "fk_message_attachments_messages_message_id",
                        column: x => x.message_id,
                        principalSchema: "cnv",
                        principalTable: "messages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_message_attachments_message_id",
                schema: "cnv",
                table: "message_attachments",
                column: "message_id");
        }
    }
}
