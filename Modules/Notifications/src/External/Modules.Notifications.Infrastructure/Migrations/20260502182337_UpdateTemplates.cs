using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Notifications.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("22222222-2222-2222-2222-222222222222") },
                column: "content",
                value: "Your request has been rejected. Rejection reason: {{reason}}");

            migrationBuilder.UpdateData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("33333333-3333-3333-3333-333333333333") },
                column: "content",
                value: "Your OTP code is: {{code}}");

            migrationBuilder.UpdateData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("44444444-4444-4444-4444-444444444444") },
                column: "content",
                value: "Your OTP code is: {{code}}");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("22222222-2222-2222-2222-222222222222") },
                column: "content",
                value: "Your request has been rejected. Rejection reason: {reason}");

            migrationBuilder.UpdateData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("33333333-3333-3333-3333-333333333333") },
                column: "content",
                value: "Your OTP code is: {code}");

            migrationBuilder.UpdateData(
                schema: "ntf",
                table: "template_langs",
                keyColumns: new[] { "lang_code", "template_id" },
                keyValues: new object[] { "en", new Guid("44444444-4444-4444-4444-444444444444") },
                column: "content",
                value: "Your OTP code is: {code}");
        }
    }
}
