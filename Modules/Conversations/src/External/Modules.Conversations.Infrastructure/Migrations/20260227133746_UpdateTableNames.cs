using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Conversations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ai_conversation_users_user_id",
                schema: "cnv",
                table: "ai_conversation");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_message_ai_conversation_conversation_id",
                schema: "cnv",
                table: "ai_message");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_message_ai_message_parent_message_id",
                schema: "cnv",
                table: "ai_message");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_consumer_message",
                schema: "cnv",
                table: "inbox_consumer_message");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ai_message",
                schema: "cnv",
                table: "ai_message");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ai_conversation",
                schema: "cnv",
                table: "ai_conversation");

            migrationBuilder.RenameTable(
                name: "inbox_consumer_message",
                schema: "cnv",
                newName: "inbox_consumer_messages",
                newSchema: "cnv");

            migrationBuilder.RenameTable(
                name: "ai_message",
                schema: "cnv",
                newName: "ai_messages",
                newSchema: "cnv");

            migrationBuilder.RenameTable(
                name: "ai_conversation",
                schema: "cnv",
                newName: "ai_conversations",
                newSchema: "cnv");

            migrationBuilder.RenameIndex(
                name: "ix_ai_message_parent_message_id",
                schema: "cnv",
                table: "ai_messages",
                newName: "ix_ai_messages_parent_message_id");

            migrationBuilder.RenameIndex(
                name: "ix_ai_message_conversation_id",
                schema: "cnv",
                table: "ai_messages",
                newName: "ix_ai_messages_conversation_id");

            migrationBuilder.RenameIndex(
                name: "ix_ai_conversation_user_id",
                schema: "cnv",
                table: "ai_conversations",
                newName: "ix_ai_conversations_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_consumer_messages",
                schema: "cnv",
                table: "inbox_consumer_messages",
                columns: new[] { "id", "handler_name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_ai_messages",
                schema: "cnv",
                table: "ai_messages",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_ai_conversations",
                schema: "cnv",
                table: "ai_conversations",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ai_conversations_users_user_id",
                schema: "cnv",
                table: "ai_conversations",
                column: "user_id",
                principalSchema: "cnv",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_messages_ai_conversations_conversation_id",
                schema: "cnv",
                table: "ai_messages",
                column: "conversation_id",
                principalSchema: "cnv",
                principalTable: "ai_conversations",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_messages_ai_messages_parent_message_id",
                schema: "cnv",
                table: "ai_messages",
                column: "parent_message_id",
                principalSchema: "cnv",
                principalTable: "ai_messages",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_ai_conversations_users_user_id",
                schema: "cnv",
                table: "ai_conversations");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_messages_ai_conversations_conversation_id",
                schema: "cnv",
                table: "ai_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_ai_messages_ai_messages_parent_message_id",
                schema: "cnv",
                table: "ai_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_inbox_consumer_messages",
                schema: "cnv",
                table: "inbox_consumer_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ai_messages",
                schema: "cnv",
                table: "ai_messages");

            migrationBuilder.DropPrimaryKey(
                name: "pk_ai_conversations",
                schema: "cnv",
                table: "ai_conversations");

            migrationBuilder.RenameTable(
                name: "inbox_consumer_messages",
                schema: "cnv",
                newName: "inbox_consumer_message",
                newSchema: "cnv");

            migrationBuilder.RenameTable(
                name: "ai_messages",
                schema: "cnv",
                newName: "ai_message",
                newSchema: "cnv");

            migrationBuilder.RenameTable(
                name: "ai_conversations",
                schema: "cnv",
                newName: "ai_conversation",
                newSchema: "cnv");

            migrationBuilder.RenameIndex(
                name: "ix_ai_messages_parent_message_id",
                schema: "cnv",
                table: "ai_message",
                newName: "ix_ai_message_parent_message_id");

            migrationBuilder.RenameIndex(
                name: "ix_ai_messages_conversation_id",
                schema: "cnv",
                table: "ai_message",
                newName: "ix_ai_message_conversation_id");

            migrationBuilder.RenameIndex(
                name: "ix_ai_conversations_user_id",
                schema: "cnv",
                table: "ai_conversation",
                newName: "ix_ai_conversation_user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_inbox_consumer_message",
                schema: "cnv",
                table: "inbox_consumer_message",
                columns: new[] { "id", "handler_name" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_ai_message",
                schema: "cnv",
                table: "ai_message",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_ai_conversation",
                schema: "cnv",
                table: "ai_conversation",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_ai_conversation_users_user_id",
                schema: "cnv",
                table: "ai_conversation",
                column: "user_id",
                principalSchema: "cnv",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_message_ai_conversation_conversation_id",
                schema: "cnv",
                table: "ai_message",
                column: "conversation_id",
                principalSchema: "cnv",
                principalTable: "ai_conversation",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_ai_message_ai_message_parent_message_id",
                schema: "cnv",
                table: "ai_message",
                column: "parent_message_id",
                principalSchema: "cnv",
                principalTable: "ai_message",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
