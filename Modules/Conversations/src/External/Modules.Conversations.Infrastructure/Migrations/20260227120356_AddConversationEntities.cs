using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Conversations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "is_archived",
                schema: "cnv",
                table: "conversation_participants",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_admin",
                schema: "cnv",
                table: "conversation_participants",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.CreateTable(
                name: "ai_conversation",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ai_conversation", x => x.id);
                    table.ForeignKey(
                        name: "fk_ai_conversation_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "cnv",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ai_message",
                schema: "cnv",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    parent_message_id = table.Column<string>(type: "text", nullable: true),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: false),
                    contnet = table.Column<string>(type: "text", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_ai_message", x => x.id);
                    table.ForeignKey(
                        name: "fk_ai_message_ai_conversation_conversation_id",
                        column: x => x.conversation_id,
                        principalSchema: "cnv",
                        principalTable: "ai_conversation",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_ai_message_ai_message_parent_message_id",
                        column: x => x.parent_message_id,
                        principalSchema: "cnv",
                        principalTable: "ai_message",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_ai_conversation_user_id",
                schema: "cnv",
                table: "ai_conversation",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_ai_message_conversation_id",
                schema: "cnv",
                table: "ai_message",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_ai_message_parent_message_id",
                schema: "cnv",
                table: "ai_message",
                column: "parent_message_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ai_message",
                schema: "cnv");

            migrationBuilder.DropTable(
                name: "ai_conversation",
                schema: "cnv");

            migrationBuilder.AlterColumn<bool>(
                name: "is_archived",
                schema: "cnv",
                table: "conversation_participants",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "is_admin",
                schema: "cnv",
                table: "conversation_participants",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);
        }
    }
}
