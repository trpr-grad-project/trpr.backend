using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_template_lang_templates_template_id",
                schema: "ntf",
                table: "template_lang");

            migrationBuilder.DropPrimaryKey(
                name: "pk_template_lang",
                schema: "ntf",
                table: "template_lang");

            migrationBuilder.RenameTable(
                name: "template_lang",
                schema: "ntf",
                newName: "template_langs",
                newSchema: "ntf");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                schema: "ntf",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                schema: "ntf",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "pk_template_langs",
                schema: "ntf",
                table: "template_langs",
                columns: new[] { "template_id", "lang_code" });

            migrationBuilder.CreateTable(
                name: "notifications",
                schema: "ntf",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<int>(type: "integer", nullable: false),
                    notify_email = table.Column<bool>(type: "boolean", nullable: false),
                    notify_phone = table.Column<bool>(type: "boolean", nullable: false),
                    notify_system = table.Column<bool>(type: "boolean", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notifications", x => x.id);
                    table.ForeignKey(
                        name: "fk_notifications_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "ntf",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_notifications_user_id",
                schema: "ntf",
                table: "notifications",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "fk_template_langs_templates_template_id",
                schema: "ntf",
                table: "template_langs",
                column: "template_id",
                principalSchema: "ntf",
                principalTable: "templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_template_langs_templates_template_id",
                schema: "ntf",
                table: "template_langs");

            migrationBuilder.DropTable(
                name: "notifications",
                schema: "ntf");

            migrationBuilder.DropPrimaryKey(
                name: "pk_template_langs",
                schema: "ntf",
                table: "template_langs");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "ntf",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "ntf",
                table: "users");

            migrationBuilder.RenameTable(
                name: "template_langs",
                schema: "ntf",
                newName: "template_lang",
                newSchema: "ntf");

            migrationBuilder.AddPrimaryKey(
                name: "pk_template_lang",
                schema: "ntf",
                table: "template_lang",
                columns: new[] { "template_id", "lang_code" });

            migrationBuilder.AddForeignKey(
                name: "fk_template_lang_templates_template_id",
                schema: "ntf",
                table: "template_lang",
                column: "template_id",
                principalSchema: "ntf",
                principalTable: "templates",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
