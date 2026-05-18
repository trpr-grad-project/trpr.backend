using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Users.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveKeycloakAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inbox_consumer_message",
                schema: "usr");

            migrationBuilder.DropColumn(
                name: "is_active",
                schema: "usr",
                table: "vibes");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "usr",
                table: "vibes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                schema: "usr",
                table: "vibes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                schema: "usr",
                table: "vibes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                schema: "usr",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                schema: "usr",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                schema: "usr",
                table: "tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                schema: "usr",
                table: "tokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                schema: "usr",
                table: "profiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                schema: "usr",
                table: "profiles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "vibe_id",
                schema: "usr",
                table: "profile_vibes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "language_id",
                schema: "usr",
                table: "profile_languages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "interest_id",
                schema: "usr",
                table: "profile_interests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "usr",
                table: "languages",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                schema: "usr",
                table: "languages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                schema: "usr",
                table: "languages",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<int>(
                name: "id",
                schema: "usr",
                table: "interests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at_utc",
                schema: "usr",
                table: "interests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at_utc",
                schema: "usr",
                table: "interests",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "guide_upgrade_request",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    rejection_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "text", nullable: false),
                    subject = table.Column<string>(type: "text", nullable: true),
                    admin_id = table.Column<int>(type: "integer", nullable: true),
                    reviewed_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_guide_upgrade_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_guide_upgrade_request_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "usr",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "inbox_consumer_messages",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    handler_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_consumer_messages", x => new { x.id, x.handler_name });
                });

            migrationBuilder.CreateTable(
                name: "document",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    guide_request_id = table.Column<Guid>(type: "uuid", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    file_url = table.Column<string>(type: "text", nullable: false),
                    created_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_document", x => x.id);
                    table.ForeignKey(
                        name: "fk_document_guide_upgrade_request_guide_request_id",
                        column: x => x.guide_request_id,
                        principalSchema: "usr",
                        principalTable: "guide_upgrade_request",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "usr",
                table: "interests",
                columns: new[] { "id", "created_at_utc", "icon", "name", "updated_at_utc" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/history.png", "History", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/adventure.png", "Adventure", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/nature.png", "Nature", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/culture.png", "Culture", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/foodie.png", "Foodie", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/wellness.png", "Wellness", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/interests/romantic.png", "Romantic", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                schema: "usr",
                table: "languages",
                columns: new[] { "id", "code", "created_at_utc", "icon", "name", "native_name", "updated_at_utc" },
                values: new object[,]
                {
                    { 1, "en", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/english.png", "English", "English", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "es", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/spanish.png", "Spanish", "Español", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "fr", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/french.png", "French", "Français", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "de", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/german.png", "German", "Deutsch", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "zh", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/chinese.png", "Chinese", "中文", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "ja", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/japanese.png", "Japanese", "日本語", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "ko", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/korean.png", "Korean", "한국어", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "pt", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/portuguese.png", "Portuguese", "Português", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, "ru", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/russian.png", "Russian", "Русский", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "ar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "@/assets/languages/arabic.png", "Arabic", "العربية", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                schema: "usr",
                table: "vibes",
                columns: new[] { "id", "created_at_utc", "description", "name", "thumbnail", "updated_at_utc" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Self Discovery and Personal Growth.", "Solo", "@/assets/vibes/solo.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Night out with friends and socializing.", "Friends", "@/assets/vibes/friends.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Intimate and romantic setting.", "Romantic", "@/assets/vibes/romantic.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kid friendly and family-oriented atmosphere.", "Family", "@/assets/vibes/family.png", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_document_guide_request_id",
                schema: "usr",
                table: "document",
                column: "guide_request_id");

            migrationBuilder.CreateIndex(
                name: "ix_guide_upgrade_request_user_id",
                schema: "usr",
                table: "guide_upgrade_request",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "document",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "inbox_consumer_messages",
                schema: "usr");

            migrationBuilder.DropTable(
                name: "guide_upgrade_request",
                schema: "usr");

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "interests",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "interests",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "interests",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "interests",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "interests",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "interests",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "interests",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "languages",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "vibes",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "vibes",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "vibes",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "usr",
                table: "vibes",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "usr",
                table: "vibes");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "usr",
                table: "vibes");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "usr",
                table: "users");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "usr",
                table: "users");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "usr",
                table: "tokens");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "usr",
                table: "tokens");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "usr",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "usr",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "usr",
                table: "languages");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "usr",
                table: "languages");

            migrationBuilder.DropColumn(
                name: "created_at_utc",
                schema: "usr",
                table: "interests");

            migrationBuilder.DropColumn(
                name: "updated_at_utc",
                schema: "usr",
                table: "interests");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "usr",
                table: "vibes",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                schema: "usr",
                table: "vibes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "vibe_id",
                schema: "usr",
                table: "profile_vibes",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "language_id",
                schema: "usr",
                table: "profile_languages",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "interest_id",
                schema: "usr",
                table: "profile_interests",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "usr",
                table: "languages",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<Guid>(
                name: "id",
                schema: "usr",
                table: "interests",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "inbox_consumer_message",
                schema: "usr",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    handler_name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_consumer_message", x => new { x.id, x.handler_name });
                });
        }
    }
}
