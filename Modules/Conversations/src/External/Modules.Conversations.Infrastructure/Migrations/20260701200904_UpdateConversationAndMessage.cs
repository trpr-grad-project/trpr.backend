using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Conversations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateConversationAndMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_users_sender_user_id",
                schema: "cnv",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "last_message_id",
                schema: "cnv",
                table: "conversations");

            migrationBuilder.DropColumn(
                name: "is_admin",
                schema: "cnv",
                table: "conversation_participants");

            migrationBuilder.DropColumn(
                name: "is_archived",
                schema: "cnv",
                table: "conversation_participants");

            migrationBuilder.AddColumn<int>(
                name: "sequence_number",
                schema: "cnv",
                table: "messages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sequence_number",
                schema: "cnv",
                table: "conversations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "fk_messages_users_sender_user_id",
                schema: "cnv",
                table: "messages",
                column: "sender_user_id",
                principalSchema: "cnv",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_messages_users_sender_user_id",
                schema: "cnv",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "sequence_number",
                schema: "cnv",
                table: "messages");

            migrationBuilder.DropColumn(
                name: "sequence_number",
                schema: "cnv",
                table: "conversations");

            migrationBuilder.AddColumn<Guid>(
                name: "last_message_id",
                schema: "cnv",
                table: "conversations",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_admin",
                schema: "cnv",
                table: "conversation_participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "is_archived",
                schema: "cnv",
                table: "conversation_participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "fk_messages_users_sender_user_id",
                schema: "cnv",
                table: "messages",
                column: "sender_user_id",
                principalSchema: "cnv",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
