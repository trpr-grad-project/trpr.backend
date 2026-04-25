using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Conversations.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixConversationIdMapping : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_conversation_participants_conversations_conversation_id1",
                schema: "cnv",
                table: "conversation_participants");

            migrationBuilder.DropIndex(
                name: "ix_conversation_participants_conversation_id1",
                schema: "cnv",
                table: "conversation_participants");

            migrationBuilder.DropColumn(
                name: "conversation_id1",
                schema: "cnv",
                table: "conversation_participants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "conversation_id1",
                schema: "cnv",
                table: "conversation_participants",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_conversation_participants_conversation_id1",
                schema: "cnv",
                table: "conversation_participants",
                column: "conversation_id1");

            migrationBuilder.AddForeignKey(
                name: "fk_conversation_participants_conversations_conversation_id1",
                schema: "cnv",
                table: "conversation_participants",
                column: "conversation_id1",
                principalSchema: "cnv",
                principalTable: "conversations",
                principalColumn: "id");
        }
    }
}
