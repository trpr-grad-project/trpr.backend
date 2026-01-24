using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Modules.Users.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "user_name");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "ix_users_user_name");

            migrationBuilder.AddColumn<string>(
                name: "avatar_url",
                table: "profiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "bio",
                table: "profiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "profiles",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "token",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    is_revoked = table.Column<bool>(type: "boolean", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_token", x => x.id);
                    table.ForeignKey(
                        name: "fk_token_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_token_user_id",
                table: "token",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "token");

            migrationBuilder.DropColumn(
                name: "avatar_url",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "bio",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "email",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "profiles");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "users",
                newName: "email");

            migrationBuilder.RenameIndex(
                name: "ix_users_user_name",
                table: "users",
                newName: "ix_users_email");
        }
    }
}
