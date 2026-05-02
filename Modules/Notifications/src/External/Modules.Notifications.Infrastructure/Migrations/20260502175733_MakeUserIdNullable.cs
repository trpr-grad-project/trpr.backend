using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_templates_users_user_id",
                schema: "ntf",
                table: "templates");

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "ntf",
                table: "templates",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.InsertData(
                schema: "ntf",
                table: "templates",
                columns: new[] { "id", "active", "content_type", "created_at_utc", "template_type", "updated_at_utc", "user_id" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("22222222-2222-2222-2222-222222222222"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("33333333-3333-3333-3333-333333333333"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { new Guid("44444444-4444-4444-4444-444444444444"), true, 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });

            migrationBuilder.InsertData(
                schema: "ntf",
                table: "template_langs",
                columns: new[] { "lang_code", "template_id", "content", "created_at_utc", "title", "updated_at_utc" },
                values: new object[,]
                {
                    { "en", new Guid("11111111-1111-1111-1111-111111111111"), "Your request has been approved.", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Approval Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "en", new Guid("22222222-2222-2222-2222-222222222222"), "Your request has been rejected. Rejection reason: {reason}", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Rejection Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "en", new Guid("33333333-3333-3333-3333-333333333333"), "Your OTP code is: {code}", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "OTP Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { "en", new Guid("44444444-4444-4444-4444-444444444444"), "Your OTP code is: {code}", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Forget Password OTP Message", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.AddForeignKey(
                name: "fk_templates_users_user_id",
                schema: "ntf",
                table: "templates",
                column: "user_id",
                principalSchema: "ntf",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_templates_users_user_id",
                schema: "ntf",
                table: "templates");

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("22222222-2222-2222-2222-222222222222") });

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("33333333-3333-3333-3333-333333333333") });

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("44444444-4444-4444-4444-444444444444") });

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "templates",
                keyColumn: "id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "templates",
                keyColumn: "id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "templates",
                keyColumn: "id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                schema: "ntf",
                table: "templates",
                keyColumn: "id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.AlterColumn<Guid>(
                name: "user_id",
                schema: "ntf",
                table: "templates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "fk_templates_users_user_id",
                schema: "ntf",
                table: "templates",
                column: "user_id",
                principalSchema: "ntf",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
